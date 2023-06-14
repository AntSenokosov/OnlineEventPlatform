using System.Net;
using Api.Responses;
using Application.Catalog;
using Application.Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalog;

[Route("types")]
public class EventTypeController : Controller
{
    private readonly IEventTypeService _typeService;

    public EventTypeController(IEventTypeService typeService)
    {
        _typeService = typeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<EventTypeDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetTypes()
    {
        var response = new ListItemsResponse<EventTypeDto>()
        {
            Items = await _typeService.GetTypes()
        };

        return Ok(response);
    }
}