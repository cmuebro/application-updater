using Common.Enums;
using UpdateStorage.Strategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UpdateStorage;

public class UpdateStorageSystem : IUpdateStorageSystem
{
    private IConfiguration Configuration { get; }
    private IServiceProvider ServiceProvider { get; }
    private FileSystemUpdateStorageStrategy FileSystemUpdateStorageStrategy { get; }
    private DatabaseUpdateStorageStrategy DatabaseUpdateStorageStrategy { get; }


    public UpdateStorageSystem(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        /* FIXME: NARG! Gefällt mir nicht... Direkt über ServiceProvider.GetService() wäre cooler */
        FileSystemUpdateStorageStrategy fileSystemUpdateStorageStrategy,
        DatabaseUpdateStorageStrategy databaseUpdateStorageStrategy)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.FileSystemUpdateStorageStrategy = fileSystemUpdateStorageStrategy ?? throw new ArgumentNullException(nameof(fileSystemUpdateStorageStrategy));
        this.DatabaseUpdateStorageStrategy = databaseUpdateStorageStrategy ?? throw new ArgumentNullException(nameof(databaseUpdateStorageStrategy));
    }

    /// <summary>
    ///     Liefert das Update-Storage-System, das in der Konfiguration angegeben ist.
    /// </summary>
    /// <returns>
    ///     Liefert eine Strategie in Form einer <see cref="IUpdateStorageStrategy"/>.
    /// </returns>
    /// <exception cref="InvalidCastException">
    ///     Tritt dann auf, wenn ein ungültiger Wert in der Konfiguration für 'UpdateStorageType' angegeben wurde.
    /// </exception>
    public IUpdateStorageStrategy? GetUpdateStorageSystem()
    {
        //return ServiceProvider.GetService<IUpdateStorageStrategy>();

        UpdateStorageTypeEnum updateStorageType = GetUpdateStorageTypeFromConfig();

        switch (updateStorageType)
        {
            case UpdateStorageTypeEnum.FileSystem:
                return ServiceProvider.GetService<FileSystemUpdateStorageStrategy>();

            case UpdateStorageTypeEnum.Database:
                return ServiceProvider.GetService<DatabaseUpdateStorageStrategy>();

            default:
                throw new InvalidCastException($"UpdateStorageType '{updateStorageType}' wird nicht unterstützt.");
        }
    }


    /// <summary>
    ///     Liefert den konfigurierten UpdateStorage-Typ aus der Konfiguration.
    /// </summary>
    /// <returns>
    ///     Liefert den UpdateStorage-Typ als <see cref="UpdateStorageTypeEnum"/>.
    /// </returns>
    private UpdateStorageTypeEnum GetUpdateStorageTypeFromConfig()
    {
        var updateStorageTypeConfig = Configuration["UpdateStorageType"];

        // Fallback soll das Dateisystem sein
        if (updateStorageTypeConfig == null)
            return UpdateStorageTypeEnum.FileSystem;

        var updateStorageType = Enum.Parse<UpdateStorageTypeEnum>(updateStorageTypeConfig);
        return updateStorageType;
    }
}
