using System.Net;
using Api.Requests.Catalog;
using Api.Responses;
using Application.Catalog;
using Application.Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalog;

[Route("speakers")]
public class SpeakerController : Controller
{
    private readonly ISpeakerService _speakerService;

    public SpeakerController(ISpeakerService speakerService)
    {
        _speakerService = speakerService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<SpeakerDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSpeakers()
    {
        var response = new ListItemsResponse<SpeakerDto>()
        {
            Items = await _speakerService.GetSpeakersAsync()
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemResponse<SpeakerDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSpeaker([FromRoute] int id)
    {
        var response = new ItemResponse<SpeakerDto>()
        {
            Item = await _speakerService.GetSpeakerAsync(id)
        };

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateSpeaker([FromBody] SpeakerCreateUpdateRequest request)
    {
        var response = new AddResponse()
        {
            Id = await _speakerService.AddSpeakerAsync(
                request.FirstName,
                request.LastName,
                request.DepartmentId,
                request.PositionId,
                request.Description)
        };

        return Ok(response);
    }

    [HttpPut("{id}/update")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateSpeaker([FromRoute] int id, [FromBody] SpeakerCreateUpdateRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _speakerService.UpdateSpeakerAsync(
                id, request.FirstName, request.LastName, request.DepartmentId, request.PositionId, request.Description)
        };

        return Ok(response);
    }

    [HttpDelete("{id}/remove")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RemoveSpeaker([FromRoute]int id)
    {
        var response = new UpdateResponse()
        {
            Id = await _speakerService.RemoveSpeakerAsync(id)
        };

        return Ok(response);
    }
}