using System.Net;
using Api.Requests.UserEvents;
using Api.Responses;
using Application.Catalog;
using Application.UserEvents.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.UserEvents;

[Route("userevents")]
public class UserEventController : Controller
{
    private readonly IUserEventService _eventService;

    public UserEventController(IUserEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<OnlineEventDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _eventService.GetEvents();

        if (events == null)
        {
            return BadRequest();
        }

        var response = new ListItemsResponse<OnlineEventDto>()
        {
            Items = events
        };

        return Ok(response);
    }

    [HttpPost("check")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CheckEvent([FromBody] AddDeleteRequest request)
    {
        var response = await _eventService.Check(request.EventId);

        return Ok(response);
    }

    [HttpPost("add")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddEvent([FromBody] AddDeleteRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _eventService.AddEvent(request.EventId)
        };

        return Ok(response);
    }

    [HttpPost("delete")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteEvent([FromBody] AddDeleteRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _eventService.DeleteEvent(request.EventId)
        };

        return Ok(response);
    }
}