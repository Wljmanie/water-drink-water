using System.Net.Http.Json;
using System.Text.Json;
using blazor.wa.tbd.Infrastructure;
using blazor.wa.tbd.Services.Responses;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.wa.tbd.Services;

public class AuthService(
    HttpClient client,
    AuthenticationStateProvider authenticationStateProvider)
{
    public async Task Logout()
    {
        await ((CustomAuthenticationStateProvider)authenticationStateProvider).Logout();
    }

    public async Task<bool> IsAuthenticated()
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();

        return state.User.Identity?.IsAuthenticated ?? false;
    }

    public async Task<string?> TryGetAuthToken()
    {
        var state = await authenticationStateProvider.GetAuthenticationStateAsync();

        var isAuthenticated = state.User.Identity?.IsAuthenticated ?? false;

        return isAuthenticated
            ? await ((CustomAuthenticationStateProvider)authenticationStateProvider).Token
            : null;
    }

    public async Task<bool> Authenticate(string email, string password)
    {
        var response = await client.PostAsJsonAsync("api/login", new { email, password });

        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var content = await response.Content.ReadAsStringAsync();

        var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (loginResponse is null)
        {
            return false;
        }

        await ((CustomAuthenticationStateProvider)authenticationStateProvider).Login(loginResponse.Token,
            loginResponse.Expiration);

        return true;
    }
}