using Common.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UpdateStorage.Strategies;

/// <summary>
///    Implementiert die UpdateStorage-Strategie, die die Programmversionen im Dateisystem ablegt.
/// </summary>
public class FileSystemUpdateStorageStrategy : IUpdateStorageStrategy
{
    private IConfiguration Configuration { get; }

    private string BasePath { get; }


    public FileSystemUpdateStorageStrategy(
        IConfiguration configuration)
    {
        Configuration = configuration;

        BasePath = Configuration["FileSystemUpdateStorage:StorageBasePath"] ?? @".\files";
    }

    /// <summary>
    ///     Liefert eine <see cref="FileDownloadResource"> mit der neuesten Programmversion, die 
    ///     aktuell im Dateisystem abgelegt ist.
    /// </summary>
    /// <returns>
    ///     Liefert die aktuellste Programmversion als <see cref="FileDownloadResource">.
    /// </returns>
    public async Task<FileDownloadResource> DownloadLatestVersion()
    {
        try
        {
            var latestExistingVersionNumber = await GetLatestVersion();
            var versionPath = GetVersionFileLocation(latestExistingVersionNumber);

            var fileBytes = await File.ReadAllBytesAsync(versionPath);

            var result = new FileDownloadResource
            {
                Version = latestExistingVersionNumber,
                FileContent = fileBytes
            };

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }


    /// <summary>
    ///     Liefert die höchste Version, die im Dateisystem abgelegt ist.
    /// </summary>
    /// <returns>
    ///     Gibt die höchste Version in Form einer <see cref="Version"/> zurück.
    /// </returns>
    /// <exception cref="DirectoryNotFoundException">
    ///     Wird geworfen, wenn <see cref="BasePath"/> nicht existiert oder bisher keine Versionen des 
    ///     Programmes im Dateisystem abgelegt sind.
    /// </exception>
    /// <remarks>
    ///     Hat zwar eine Warning wegen async, aber die Methode muss async sein, da auf sie in einem 
    ///     asynchronen Kontext zugegriffen wird. Grund dafür sind die unterschiedlichen Implementierungen 
    ///     der UpdateStorage-Strategien.
    /// </remarks>
    public async Task<Version> GetLatestVersion()
    {
        if (!Directory.Exists(BasePath))
            throw new DirectoryNotFoundException("Das Verzeichnis für die Programmversionen wurde nicht gefunden.");

        var highestVersion =
            Directory.GetDirectories(BasePath)
                .Select(versionDir => Version.Parse(new DirectoryInfo(versionDir).Name))
                .OrderDescending()
                .FirstOrDefault();

        if (highestVersion == null)
            throw new DirectoryNotFoundException("Es wurde kein Verzeichnis gefunden, dessen Pfad als Versionsnummer interpretierbar wäre.");

        return highestVersion;
    }


    /// <summary>
    ///     Liefer den Pfad zur ausführbaren Datei einer bestimmten Version.
    /// </summary>
    /// <param name="version"></param>
    /// <exception cref="FileNotFoundException">
    ///     Wird geworfen, wenn an der erwarteten Stelle keine passende Datei gefunden wurde.
    /// </exception>
    /// <returns>
    ///     Liefert den Pfad zur ausführbaren Datei für die angegebenen Version.
    /// </returns>
    private string GetVersionFileLocation(Version version)
    {
        var path = string.Join(@"\", BasePath, version.ToString(), "SampleApplication.exe");

        // prüfen, ob die Datei existiert
        if (!File.Exists(path))
            throw new FileNotFoundException(path);

        return path;
    }
}
