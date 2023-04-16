using System.Net;
using Api.Requests.Identity;
using Api.Responses;
using Application.Identity;
using Application.Identity.Services.Interfaces;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Identity;

[Route("userprofile")]
[Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
public class UserProfileController : Controller
{
    private readonly IUserProfileService _profileService;

    public UserProfileController(IUserProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ItemResponse<UserProfileDto>), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> GetUserProfile()
    {
        var response = new ItemResponse<UserProfileDto>()
        {
            Item = await _profileService.GetUserProfileAsync()
        };

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> CreateUserProfile([FromBody] CreateUpdateUserProfileRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _profileService.CreateUserProfileAsync(request.FirstName, request.LastName, request.Phone!),
        };

        return Ok(response);
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(UpdateResponse), (int)HttpStatusCode.OK)]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public async Task<IActionResult> UpdateUserProfile([FromBody] CreateUpdateUserProfileRequest request)
    {
        var response = new UpdateResponse()
        {
            Id = await _profileService.UpdateUserProfileAsync(request.FirstName, request.LastName, request.Phone!)
        };

        return Ok(response);
    }
}