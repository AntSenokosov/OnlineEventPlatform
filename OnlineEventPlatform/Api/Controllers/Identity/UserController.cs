using System.Net;
using Api.Requests.Identity;
using Api.Responses;
using Application.Identity;
using Application.Identity.Services.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Identity;

[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var token = await _userService.Login(loginRequest.Email, loginRequest.Password, loginRequest.GoogleAuthCode);

        var response = new LoginResponse()
        {
            Token = token
        };

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ItemResponse<UserDto>), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> GetUser()
    {
        var response = new ItemResponse<UserDto>()
        {
            Item = await _userService.GetUserAsync()
        };

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(AddResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var response = new AddResponse()
        {
            Id = await _userService.CreateUser(request.Email, request.Password)
        };

        return Ok(response);
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _userService.UpdateUser(request.Email, request.Password, request.GoogleAuthCode)
        };

        return Ok(response);
    }

    [HttpDelete("delete")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> DeleteUser()
    {
        var response = await _userService.DeleteUser();

        return Ok(response);
    }

    [HttpPost("generateTwoFactory")]
    [ProducesResponseType(typeof(GenerateTwoFactoryResponse), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> GenerateTwoFactory([FromBody] GenerateTwoFactoryRequest request)
    {
        var response = await _userService.GenerateTwoFactorAuth(request.Retry, request.Password);

        if (response == null)
        {
            return Ok();
        }

        var result = new GenerateTwoFactoryResponse()
        {
            QrCodeImageUrl = response.QrCodeSetupImageUrl,
            ManualEntrySetupCode = response.ManualEntryKey
        };

        return Ok(result);
    }

    [HttpPost("verify")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> VerifyTwoFactory([FromBody] VerifyTwoFactoryRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _userService.VerifyTwoFactorAuth(request.GoogleAuthCode)
        };

        return Ok(response);
    }

    [HttpPut("disable")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> DisableTwoFactory([FromBody] DisableTwoFactoryRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _userService.DisableTwoFactorAuth(request.Password, request.GoogleAuthCode)
        };

        return Ok(response);
    }

    [HttpGet("check")]
    [ProducesResponseType(typeof(CheckTwoFactoryResponse), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> CheckTwoFactory()
    {
        var response = new CheckTwoFactoryResponse()
        {
            Enable = await _userService.CheckTwoFactorAuth()
        };

        return Ok(response);
    }
}