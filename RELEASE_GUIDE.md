# Guía de Publicación de Versiones

Para publicar una nueva versión de **jellyfin-reports** y que esté disponible para tus usuarios, sigue estos pasos:

## 1. Preparar el Manifiesto
Antes de subir una nueva versión, debes actualizar el archivo `manifest.json` en la raíz del repositorio:
1. Incrementa la versión en el campo `"version"` (ej: `1.0.1.0`).
2. Actualiza el `"changelog"` con los cambios realizados.
3. Asegúrate de que la `"sourceUrl"` apunte a la URL correcta del futuro release (el workflow usará el tag para el nombre del archivo).
4. El campo `"timestamp"` debe reflejar la fecha actual.
5. El campo `"checksum"` se puede dejar vacío inicialmente o calcularse manualmente si es necesario (Jellyfin a veces lo ignora si la descarga es HTTPS confiable).

## 2. Crear un Tag de Git
El workflow de GitHub Actions está configurado para ejecutarse cuando creas un tag que empiece por `v` (ej: `v1.0.1.0`).

Ejecuta desde tu terminal:
```bash
git add manifest.json
git commit -m "Preparando versión 1.0.1.0"
git tag -a v1.0.1.0 -m "Versión 1.0.1.0"
git push origin main --tags
```

## 3. GitHub Actions
El workflow `.github/workflows/release.yml` se activará automáticamente:
- Compilará el código en modo **Release**.
- Empaquetará los archivos en `jellyfin-reports.zip`.
- Creará una **GitHub Release** con el nombre del tag.
- Subirá el archivo ZIP a la sección de Assets de la release.

## 4. Repositorio en Jellyfin
Una vez completado el paso anterior, los usuarios que tengan añadida la URL de tu `manifest.json` verán la actualización disponible en su panel de Jellyfin.

---
> [!TIP]
> Mantén siempre actualizado el archivo `manifest.json` junto con cada nuevo tag para que tus usuarios reciban las actualizaciones automáticamente.
