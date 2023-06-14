using System.Net;
using Api.Requests.Identity;
using Api.Responses;
using Application.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Identity;

[Route("auth")]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await _authenticationService.Login(loginRequest.Email, loginRequest.Password, loginRequest.GoogleAuthCode);

        var response = new UserResponse.UserContainer()
        {
            Id = result.Id,
            Email = result.Email,
            Token = result.Token,
            TokenValidTo = result.TokenValidTo,
            FirstName = result.FirstName,
            LastName = result.LastName,
            IsAdmin = result.IsAdmin,
            IsSuperAdmin = result.IsSuperAdmin,
            HasTwoFactoryAuthEnable = result.HasTwoFactoryAuthEnable
        };

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var response = new AddResponse()
        {
            Id = await _authenticationService.RegisterAsync(request.FirstName, request.LastName, request.Email, request.Password)
        };

        return Ok(response);
    }

    [HttpGet("check")]
    [ProducesResponseType(typeof(CheckTwoFactoryResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CheckTwoFactory([FromRoute] string email)
    {
        var response = new CheckTwoFactoryResponse()
        {
            Enable = await _authenticationService.CheckTwoFactory(email)
        };

        return Ok(response);
    }

    [HttpPost("recovery")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> PasswordRecovery([FromBody] PasswordRecoveryRequest request)
    {
        var response = await _authenticationService.PasswordRecovery(request.Email);

        return Ok(response);
    }
}