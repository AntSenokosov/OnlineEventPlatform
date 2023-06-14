using System.Net;
using Api.Requests.Identity;
using Api.Responses;
using Application.Identity;
using Application.Identity.Services.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Identity;

[Route("profile")]
[Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
public class UserProfileController : Controller
{
    private readonly IUserProfileService _profileService;

    public UserProfileController(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUserProfile()
    {
        var result = await _profileService.GetCurrentUserAsync();

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

    [HttpPut("update")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _profileService.UpdateUserAsync(request.Email, request.FirstName, request.LastName, request.GoogleAuthCode)
        };

        return Ok(response);
    }

    [HttpPut("changePassword")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword request)
    {
        var response = new UpdateResponse()
        {
            Id = await _profileService.ChangePassword(request.Password)
        };

        return Ok(response);
    }

    [HttpDelete("delete")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteUser()
    {
        var response = await _profileService.DeleteUser();

        return Ok(response);
    }

    [HttpPost("generateTwoFactory")]
    [ProducesResponseType(typeof(GenerateTwoFactoryResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GenerateTwoFactory([FromBody] GenerateTwoFactoryRequest request)
    {
        var response = await _profileService.GenerateTwoFactorAuth(request.Retry, request.Password);

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
    public async Task<IActionResult> VerifyTwoFactory([FromBody] VerifyTwoFactoryRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _profileService.VerifyTwoFactorAuth(request.GoogleAuthCode)
        };

        return Ok(response);
    }

    [HttpPut("disable")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DisableTwoFactory([FromBody] DisableTwoFactoryRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _profileService.DisableTwoFactorAuth(request.Password, request.GoogleAuthCode)
        };

        return Ok(response);
    }
}