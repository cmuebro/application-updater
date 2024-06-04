using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;

class Program
{
    private static HttpClient HttpClient { get; set; } = new HttpClient();

    private static IConfiguration? Configuration { get; set; }

    private static string? UpdateServiceBaseUrl { get; set; }

    private static string? TargetPath { get; set; }


    static async Task Main(string[] args)
    {
        // Konfiguration laden
        Configuration =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

        // URI der UpdateService-API und Zielverzeichnis aus der Konfiguration abrufen
        UpdateServiceBaseUrl = Configuration["UpdateService:BaseUrl"];
        TargetPath = Configuration["TargetPath"];

        // Aktuelle Version laden
        var currentVersion = GetCurrentProgramVersion();

        // Neueste Version vom UpdateService abfragen
        var latestVersion = await GetLatestVersionNumber();

        if (latestVersion == null)
        {
            Console.WriteLine("Fehler beim Abrufen der neuesten Version.");
            return;
        }

        // Prüfen, ob ein Update verfügbar ist
        if (currentVersion == null || latestVersion > currentVersion)
        {
            Console.WriteLine($"Neue Version verfügbar: {latestVersion}");

            // Neue Version herunterladen und installieren
            var downloadServiceUrl = $"{UpdateServiceBaseUrl}/download";

            await DownloadAndUpdateApplication(downloadServiceUrl);

            // Anwendung starten
            StartApplication();
        }
        else
        {
            Console.WriteLine("Das Programm ist bereits auf dem neuesten Stand.");
        }
    }


    private static Version? GetCurrentProgramVersion()
    {
        FileVersionInfo fileVersionInfo; 

        try
        {
            fileVersionInfo = FileVersionInfo.GetVersionInfo(string.Join(@"\", TargetPath, "SampleApplication.exe"));
        }
        catch (FileNotFoundException)
        {
            return null;
        }

        if (fileVersionInfo?.ProductVersion == null)
            return null;

        return Version.Parse(fileVersionInfo.ProductVersion);
    }


    private static async Task<Version> GetLatestVersionNumber()
    {
        try
        {
            var response = await HttpClient.GetAsync($"{UpdateServiceBaseUrl}/latest-version");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var version = JsonConvert.DeserializeObject<Version>(responseString);

            if (version == null)
                throw new Exception("Fehler beim Deserialisieren der Versionsnummer.");

            return version;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Abrufen der neuesten Version: {ex.Message}");
            throw;
        }
    }


    private static async Task DownloadAndUpdateApplication(string downloadUrl)
    {
        try
        {
            if (Configuration == null)
                throw new Exception("Konfiguration wurde nicht geladen.");

            var targetPath = Configuration["TargetPath"];
            var response = await HttpClient.GetAsync(downloadUrl);

            response.EnsureSuccessStatusCode();

            await using (var fileStream = new FileStream(string.Join(@"\", targetPath, "SampleApplication.exe"), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await response.Content.CopyToAsync(fileStream);
            }

            Console.WriteLine($"Anwendung wurde erfolgreich heruntergeladen und installiert: {targetPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Herunterladen und Installieren der Anwendung: {ex.Message}");
            throw;
        }
    }

    private static void StartApplication()
    {
        try
        {
            if (Configuration == null)
                throw new Exception("Konfiguration wurde nicht geladen.");

            var targetPath = Configuration["TargetPath"];

            System.Diagnostics.Process.Start(string.Join(@"\", targetPath, "SampleApplication.exe"));
            Console.WriteLine("Die Anwendung wurde gestartet.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Starten der Anwendung: {ex.Message}");
            throw;
        }
    }
}
