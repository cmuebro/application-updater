using Common.Resources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UpdateStorage.Strategies;

/// <summary>
///     Implementiert die UpdateStorage-Strategie, die die Programmversionen in einer Datenbank ablegt.
/// </summary>
public class DatabaseUpdateStorageStrategy : IUpdateStorageStrategy
{
    private IConfiguration Configuration { get; }


    public DatabaseUpdateStorageStrategy(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    Task<Version> IUpdateStorageStrategy.GetLatestVersion()
    {
        throw new NotImplementedException();
    }

    Task<FileDownloadResource> IUpdateStorageStrategy.DownloadLatestVersion()
    {
        throw new NotImplementedException();
    }
}
