using System.Net;
using Api.Requests.Catalog;
using Api.Responses;
using Application.Catalog;
using Application.Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalog;

[ApiController]
[Route("positions")]
public class PositionController : Controller
{
    private readonly IPositionService _positionService;

    public PositionController(IPositionService positionService)
    {
        _positionService = positionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<PositionDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPositions()
    {
        var response = new ListItemsResponse<PositionDto>()
        {
            Items = await _positionService.GetPositionsAsync()
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemResponse<PositionDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetPosition([FromRoute] int id)
    {
        var response = new ItemResponse<PositionDto>()
        {
            Item = await _positionService.GetPositionAsync(id)
        };

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreatePosition([FromBody] PositionCreateUpdateRequest request)
    {
        var response = new AddResponse()
        {
            Id = await _positionService.AddPositionAsync(request.Name)
        };

        return Ok(response);
    }

    [HttpPut("{id}/update")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdatePosition([FromRoute] int id, [FromBody] PositionCreateUpdateRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _positionService.UpdatePositionAsync(id, request.Name)
        };

        return Ok(response);
    }

    [HttpDelete("{id}/remove")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RemovePosition([FromRoute] int id)
    {
        var response = new UpdateResponse()
        {
            Id = await _positionService.RemovePositionAsync(id)
        };

        return Ok(response);
    }
}