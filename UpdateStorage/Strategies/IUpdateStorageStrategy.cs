using Common.Resources;

namespace UpdateStorage.Strategies;

/// <summary>
///     Strategie für den Zugriff auf den UpdateStorage.
/// </summary>
public interface IUpdateStorageStrategy
{
    /// <summary>
    ///     Liefert die höchste Versionsnummer, die im Storage abgelegt ist.
    /// </summary>
    /// <returns>
    ///     Gibt die höchste Version in Form einer <see cref="Version"/> zurück.
    /// </returns>
    /// <exception cref="DirectoryNotFoundException">
    ///     Wird geworfen, wenn bisher keine Versionen des Programmes im Storage abgelegt sind.
    /// </exception>
    Task<Version> GetLatestVersion();

    /// <summary>
    ///     Liefert die höchste Version, die im Storage abgelegt ist.
    /// </summary>
    /// <returns>
    ///     Gibt die höchste Version in Form einer <see cref="Version"/> zurück.
    /// </returns>
    /// <exception cref="DirectoryNotFoundException">
    ///     Wird geworfen, wenn <see cref="BasePath"/> nicht existiert oder bisher keine Versionen des 
    ///     Programmes im gewählten Storage abgelegt sind.
    /// </exception>
    /// <remarks>
    ///     Hat zwar eine Warning wegen async, aber die Methode muss async sein, da auf sie in einem 
    ///     asynchronen Kontext zugegriffen wird. Grund dafür sind die unterschiedlichen Implementierungen 
    ///     der UpdateStorage-Strategien.
    /// </remarks>
    Task<FileDownloadResource> DownloadLatestVersion();
}