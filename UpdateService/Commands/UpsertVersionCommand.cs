using MediatR;

namespace UpdateService.Commands;

public class UpsertVersionCommand : IRequest<Version> { }

public class UpsertVersionCommandHandler : IRequestHandler<UpsertVersionCommand, Version>
{
    public async Task<Version> Handle(UpsertVersionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
