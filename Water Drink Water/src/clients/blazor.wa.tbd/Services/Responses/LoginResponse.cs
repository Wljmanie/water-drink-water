namespace blazor.wa.tbd.Services.Responses;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public long Expiration { get; set; }
}