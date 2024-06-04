using MediatR;
using UpdateStorage;

namespace UpdateService.Queries;

public class GetLatestVersionQuery : IRequest<Version> { }

public class GetLatestVersionQueryHandler : IRequestHandler<GetLatestVersionQuery, Version>
{
    public IUpdateStorageSystem UpdateStorageSystem { get; }

    public GetLatestVersionQueryHandler(IUpdateStorageSystem updateStorageSystem)
    {
        UpdateStorageSystem = updateStorageSystem;
    }

    public async Task<Version> Handle(GetLatestVersionQuery request, CancellationToken cancellationToken)
    {
        var updateStorage = UpdateStorageSystem.GetUpdateStorageSystem();

        if (updateStorage == null)
            throw new Exception("UpdateStorage ist nicht verfügbar.");

        var version = await updateStorage.GetLatestVersion();

        if (version == null)
            throw new Exception("Es wurde noch keine Programmversion abgelegt.");

        return version;
    }
}
