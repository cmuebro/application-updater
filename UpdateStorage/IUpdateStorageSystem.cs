using UpdateStorage.Strategies;

namespace UpdateStorage;

/// <summary>
///     Zugriff auf das UpdateStorage-System.
/// </summary>
/// <remarks>
///     Ich hab das hier über ein Strategie-System gelöst, damit bei jedem Aufruf die Config ausgelesen wird und
///     neu entschieden werden kann, welches UpdateStorage-System verwendet wird. 
/// </remarks>
public interface IUpdateStorageSystem
{
    /// <summary>
    ///     Liefert die UpdateStorage-Strategie, die in der Konfiguration angegeben ist.
    /// </summary>  
    IUpdateStorageStrategy GetUpdateStorageSystem();
}
