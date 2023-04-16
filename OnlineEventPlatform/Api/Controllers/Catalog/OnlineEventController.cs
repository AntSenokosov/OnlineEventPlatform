using System.Net;
using Api.Requests.Catalog;
using Api.Responses;
using Application.Catalog;
using Application.Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalog;

[Route("onlineevents")]
public class OnlineEventController : Controller
{
    private readonly IOnlineEventService _eventService;

    public OnlineEventController(IOnlineEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<OnlineEventDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetEvents()
    {
        var response = new ListItemsResponse<OnlineEventDto>()
        {
            Items = await _eventService.GetOnlineEventsAsync(),
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemResponse<OnlineEventDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetEvent([FromRoute] int id)
    {
        var response = new ItemResponse<OnlineEventDto>()
        {
            Item = await _eventService.GetOnlineEventAsync(id)
        };

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateEvent([FromBody] OnlineEventCreateUpdateRequest request)
    {
        var response = new AddResponse()
        {
            Id = await _eventService.AddOnlineEventAsync(
                request.Name,
                request.Description,
                request.DateAndTime,
                request.AboutEvent)
        };

        return Ok(response);
    }

    [HttpPut("{id}/update")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromBody] OnlineEventCreateUpdateRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _eventService.UpdateOnlineEventAsync(
                    id,
                    request.Name,
                    request.Description,
                    request.DateAndTime,
                    request.AboutEvent)
        };

        return Ok(response);
    }

    [HttpDelete("{id}/remove")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RemoveEvent([FromRoute] int id)
    {
        var response = new UpdateResponse()
        {
            Id = await _eventService.RemoveOnlineEventAsync(id)
        };

        return Ok(response);
    }
}