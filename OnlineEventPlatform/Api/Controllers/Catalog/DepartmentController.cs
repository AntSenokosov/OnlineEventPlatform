using System.Net;
using Api.Requests.Catalog;
using Api.Responses;
using Application.Catalog;
using Application.Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Catalog;

[Route("departments")]
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<DepartmentDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDepartments()
    {
        var response = new ListItemsResponse<DepartmentDto>()
        {
            Items = await _departmentService.GetDepartmentsAsync()
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemResponse<DepartmentDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDepartment([FromRoute] int id)
    {
        var response = new ItemResponse<DepartmentDto>()
        {
            Item = await _departmentService.GetDepartmentAsync(id)
        };

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateUpdateRequest request)
    {
        var response = new AddResponse()
        {
            Id = await _departmentService.AddDepartmentAsync(request.Number, request.Name),
        };

        return Ok(response);
    }

    [HttpPut("{id}/update")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateDepartment([FromRoute]int id, [FromBody] DepartmentCreateUpdateRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _departmentService.UpdateDepartmentAsync(id, request.Number, request.Name)
        };

        return Ok(response);
    }

    [HttpDelete("{id}/remove")]
    [ProducesResponseType(typeof(ItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> RemoveDepartment([FromRoute] int id)
    {
        var response = new ItemResponse<int?>()
        {
            Item = await _departmentService.RemoveDepartmentAsync(id)
        };

        return Ok(response);
    }
}