namespace Api.Responses;

public class GenerateTwoFactoryResponse
{
    public string QrCodeImageUrl { get; set; } = null!;
    public string ManualEntrySetupCode { get; set; } = null!;
}