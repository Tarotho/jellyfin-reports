using System;
using System.Collections.Generic;
using System.IO;
using Jellyfin.Plugin.Reports.Configuration;
using Jellyfin.Plugin.Reports.Grid;
using Jellyfin.Plugin.Reports.Models;
using Jellyfin.Plugin.Reports.Modules;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;

namespace GridTest;

// Simple wrapper class to avoid Jellyfin dependencies
public class SimplePluginConfig
{
    public List<ModuleDefinition> Modules { get; set; } = new List<ModuleDefinition>();
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Generando reporte de prueba...");

        // Configure QuestPDF license (Community license for testing)
        QuestPDF.Settings.License = LicenseType.Community;

        // Create test configuration
        var modules = new List<ModuleDefinition>
        {
            // First mock module at (0,0) with size 1x1
            new ModuleDefinition
            {
                ModuleId = "Mock",
                X = 0,
                Y = 0,
                W = 1,
                H = 1,
                Settings = new Dictionary<string, string>
                {
                    { "Color", "#E74C3C" } // Red
                }
            },
            // Second mock module at (0,1) with size 2x2
            new ModuleDefinition
            {
                ModuleId = "Mock",
                X = 0,
                Y = 1,
                W = 2,
                H = 2,
                Settings = new Dictionary<string, string>
                {
                    { "Color", "#27AE60" } // Green
                }
            }
        };

        // Validate configuration
        var errors = GridValidator.Validate(modules);
        if (errors.Count > 0)
        {
            Console.WriteLine("❌ Errores de validación:");
            foreach (var error in errors)
            {
                Console.WriteLine($"   - {error}");
            }
            return;
        }

        // Generate PDF directly
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(QuestPDF.Helpers.PageSizes.A4);
                page.Margin(GridConstants.DefaultMarginPoints);

                page.Content().Table(table =>
                {
                    // Define columns
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < GridConstants.GridColumns; i++)
                        {
                            columns.RelativeColumn();
                        }
                    });

                    // Create modules and render them
                    var sortedModules = modules
                        .Select(CreateModule)
                        .OrderBy(m => m.Row)
                        .ThenBy(m => m.Column)
                        .ToList();

                    foreach (var module in sortedModules)
                    {
                        table.Cell()
                            .Row((uint)(module.Row + 1))
                            .Column((uint)(module.Column + 1))
                            .RowSpan((uint)module.RowSpan)
                            .ColumnSpan((uint)module.ColumnSpan)
                            .Element(c => module.Render(c, new UserAnnualStats()));
                    }

                    // Fill empty cells
                    var occupiedCells = new bool[GridConstants.GridRows][];
                    for (int i = 0; i < GridConstants.GridRows; i++)
                    {
                        occupiedCells[i] = new bool[GridConstants.GridColumns];
                    }

                    foreach (var module in sortedModules)
                    {
                        for (int r = module.Row; r < module.Row + module.RowSpan; r++)
                        {
                            for (int c = module.Column; c < module.Column + module.ColumnSpan; c++)
                            {
                                occupiedCells[r][c] = true;
                            }
                        }
                    }

                    for (int row = 0; row < GridConstants.GridRows; row++)
                    {
                        for (int col = 0; col < GridConstants.GridColumns; col++)
                        {
                            if (!occupiedCells[row][col])
                            {
                                table.Cell()
                                    .Row((uint)(row + 1))
                                    .Column((uint)(col + 1))
                                    .Border(1)
                                    .BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten3)
                                    .Background(QuestPDF.Helpers.Colors.Grey.Lighten5);
                            }
                        }
                    }
                });
            });
        });

        byte[] pdfBytes = document.GeneratePdf();

        // Save to file
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "test-report.pdf");
        File.WriteAllBytes(outputPath, pdfBytes);

        Console.WriteLine($"✅ Reporte generado exitosamente: {outputPath}");
        Console.WriteLine($"   Tamaño del archivo: {pdfBytes.Length / 1024} KB");
        Console.WriteLine();
        Console.WriteLine("Configuración del reporte:");
        Console.WriteLine("  - Módulo 1: Posición (0,0), Tamaño 1x1, Color Rojo");
        Console.WriteLine("  - Módulo 2: Posición (0,1), Tamaño 2x2, Color Verde");
    }

    static IReportModule CreateModule(ModuleDefinition definition)
    {
        return new MockModule(
            definition.ModuleId,
            definition.X,
            definition.Y,
            definition.W,
            definition.H,
            definition.Settings);
    }
}
