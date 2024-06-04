using Common.Resources;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using UpdateService.Queries;

namespace UpdateService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UpdateServiceController : ControllerBase
{
    private IMediator Mediator { get; }

    public UpdateServiceController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    ///     Über diesen Enpoint kann die neuste Version der Software heruntergeladen werden.
    /// </summary>
    [HttpGet("download")]
    public async Task<IActionResult> DownloadLatestVersion()
    {
        var result = await Mediator.Send(new DownloadLatestVersionQuery());
        
        return File(result.FileContent, "application/octet-stream");
    }

    /// <summary>
    ///    Über diesen Endpoint kann die aktuellste Version der Software abgefragt werden.
    /// </summary>
    [HttpGet("latest-version")]
    public async Task<ActionResult<Version>> GetLatestVersion()
    {

        return Ok(await Mediator.Send(new GetLatestVersionQuery()));
    }

    ///// <summary>
    /////    Über diesen Endpoint kann eine neue Version der Software abgelegt werden.
    ///// </summary>
    //[HttpPost("{version}")]
    //public async void UpsertVersion([FromRoute] Version version, [FromBody] byte[] fileContent)
    //{
    //    return Ok(await Mediator.Send(new UpsertVersionCommand()));
    //}

}
