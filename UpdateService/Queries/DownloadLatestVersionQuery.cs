using Common.Resources;
using MediatR;
using UpdateStorage;

namespace UpdateService.Queries;

public class DownloadLatestVersionQuery : IRequest<FileDownloadResource> { }

public class DownloadLatestVersionQueryHandler : IRequestHandler<DownloadLatestVersionQuery, FileDownloadResource>
{
    public IUpdateStorageSystem UpdateStorageSystem { get; }

    public DownloadLatestVersionQueryHandler(IUpdateStorageSystem updateStorageSystem)
    {
        UpdateStorageSystem = updateStorageSystem;
    }

    public async Task<FileDownloadResource> Handle(DownloadLatestVersionQuery request, CancellationToken cancellationToken)
    {
        var updateStorage = UpdateStorageSystem.GetUpdateStorageSystem();

        if (updateStorage == null)
            throw new Exception("UpdateStorage is not available.");

        var fileDownloadResource = await updateStorage.DownloadLatestVersion();

        if (fileDownloadResource == null)
            throw new Exception("No latest version is not available.");

        return fileDownloadResource;
    }
}
