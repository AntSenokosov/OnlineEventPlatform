using System.Net;
using Api.Responses;
using Application.Catalog;
using Application.Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalog;

[Route("platforms")]
public class MeetingPlatformController : Controller
{
    private readonly IMeetingPlatformService _platformService;

    public MeetingPlatformController(IMeetingPlatformService platformService)
    {
        _platformService = platformService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<MeetingPlatformDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPlatforms()
    {
        var response = new ListItemsResponse<MeetingPlatformDto>()
        {
            Items = await _platformService.GetPlatforms()
        };

        return Ok(response);
    }
}