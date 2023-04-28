namespace Infrastructure.Services.Interfaces;

public interface ICurrentUserAccessor
{
    public string? GetCurrentEmail();
}