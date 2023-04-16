using System.Security.Claims;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(u => u.Type == ClaimTypes.Email)?.Value;
    }
}