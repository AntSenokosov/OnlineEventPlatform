using System.Net;
using Api.Responses;
using Application.Catalog;
using Application.Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalog;

[Route("catalog")]
public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<OnlineEventDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetCatalog()
    {
        var response = await _catalogService.GetCatalog();

        return Ok(response);
    }
}