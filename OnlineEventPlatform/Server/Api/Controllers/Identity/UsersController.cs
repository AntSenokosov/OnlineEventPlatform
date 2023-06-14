using System.Net;
using Api.Requests.Identity;
using Api.Responses;
using Application.Identity;
using Application.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Identity;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListItemsResponse<UserDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUsers()
    {
        var response = new ListItemsResponse<UserDto>()
        {
            Items = await _userService.GetUsers()
        };

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var response = new AddResponse()
        {
            Id = await _userService.CreateUser(request.FirstName, request.LastName, request.Email, request.Password)
        };

        return Ok(response);
    }

    [HttpGet("{id}/toAdmin")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateUserToAdmin([FromRoute] int id)
    {
        var response = new AddResponse()
        {
            Id = await _userService.UpdateUserPermissionsToAdmin(id)
        };

        return Ok(response);
    }

    [HttpGet("{id}/toUser")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateUserToUser([FromRoute] int id)
    {
        var response = new AddResponse()
        {
            Id = await _userService.UpdateUserPermissionsToUser(id)
        };

        return Ok(response);
    }

    [HttpDelete("{id}/delete")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        var response = await _userService.DeleteUser(id);

        return Ok(response);
    }
}