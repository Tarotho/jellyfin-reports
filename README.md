# 🎬 JellyfinReport - Jellyfin Annual Insights

![Jellyfin Report Header](https://raw.githubusercontent.com/tu-usuario/JellyfinReport/main/assets/header.png)

**JellyfinReport** es un plugin para Jellyfin diseñado para generar hermosos reportes anuales en formato PDF (A4). Basado en un sistema de cuadrícula flexible, permite visualizar tus estadísticas de visualización, tendencias mensuales y hitos cinematográficos de una manera elegante y profesional.

---

## ✨ Características Principales

- 📊 **Reportes PDF A4**: Genera documentos listos para imprimir o compartir.
- 🧱 **Sistema de Grid 2x10**: Configuración de layout altamente personalizable mediante JSON.
- 📈 **Estadísticas Reales**: Integración directa con los datos de tu servidor Jellyfin (tiempo de visualización, conteo de películas, series y episodios).
- 🧩 **Arquitectura Modular**: Añade nuevos módulos de visualización fácilmente.
- ⚡ **API REST**: Endpoint dedicado para generar y descargar reportes programáticamente.
- 🛠️ **Panel de Configuración**: Interfaz integrada en el dashboard de Jellyfin para ajustar el diseño.

---

## 🚀 Instalación

### Desde Binarios (Próximamente)
1. Descarga el archivo `JellyfinReport.dll` desde la sección de [Releases](https://github.com/tu-usuario/JellyfinReport/releases).
2. Colócalo en la carpeta `plugins/JellyfinReport` de tu servidor Jellyfin.
3. Reinicia Jellyfin.

### Desde el Código Fuente
1. Clona este repositorio:
   ```bash
   git clone https://github.com/tu-usuario/JellyfinReport.git
   ```
2. Asegúrate de tener instalado el SDK de .NET 10.0.
3. Compila el proyecto:
   ```bash
   dotnet build -c Release
   ```
4. Copia el archivo generado en `bin/Release/net10.0/JellyfinReport.dll` a tu carpeta de plugins de Jellyfin.

---

## ⚙️ Configuración del Layout

El plugin permite definir qué módulos aparecen en el reporte y dónde se ubican mediante una configuración JSON.

### El Sistema de Grid
El reporte utiliza una cuadrícula de **2 columnas (X: 0-1)** y **10 filas (Y: 0-9)**.

| X=0 | X=1 |
|:---:|:---:|
| (0,0) | (1,0) |
| ... | ... |
| (0,9) | (1,9) |

### Ejemplo de Configuración JSON
Puedes editar esto desde el Dashboard de Jellyfin -> Plugins -> **JellyfinReport**.

```json
{
  "Modules": [
    {
      "ModuleId": "TotalStats",
      "X": 0,
      "Y": 0,
      "W": 2,
      "H": 1,
      "Settings": {
        "Title": "Mi Año en Jellyfin 2024",
        "BackgroundColor": "#1A1A2E",
        "AccentColor": "#E94560"
      }
    },
    {
      "ModuleId": "Mock",
      "X": 0,
      "Y": 1,
      "W": 1,
      "H": 2,
      "Settings": {
        "Title": "Tendencias Mensuales",
        "Color": "#3498DB"
      }
    }
  ]
}
```

---

## 📡 Uso de la API

El plugin expone un endpoint para generar reportes bajo demanda:

### Generar Reporte PDF
- **URL**: `/AnnualReport/{UserId}`
- **Método**: `GET`
- **Parámetros Query**:
  - `year`: (Opcional) El año del reporte. Por defecto es el año actual.

**Ejemplo con cURL:**
```bash
curl -X GET "http://tu-servidor:8096/AnnualReport/TU_USER_ID" \
     -H "X-Emby-Token: TU_API_KEY" \
     --output mi_reporte.pdf
```

---

## 🛠️ Desarrollo Tecnológico

- **Core**: .NET 10.0
- **PDF Engine**: [QuestPDF](https://www.questpdf.com/)
- **UI**: HTML/Javascript (integrado en Jellyfin Dashboard)
- **API**: ASP.NET Core (Jellyfin API Controllers)

---

## 🤝 Contribuciones

¡Las contribuciones son bienvenidas! Si tienes ideas para nuevos módulos (por ejemplo, "Géneros más vistos", "Directores favoritos", etc.), no dudes en abrir un Issue o un Pull Request.

---

## 📄 Licencia

Este proyecto está bajo la licencia MIT.

---
*Hecho con ❤️ para la comunidad de Jellyfin.*
