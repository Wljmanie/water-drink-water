using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.wa.tbd.Infrastructure;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
    : AuthenticationStateProvider
{
    private readonly Lazy<ValueTask<string>> _token = new(value: localStorageService.GetItemAsync<string>("token"));
    public ValueTask<string> Token => _token.Value;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorageService.GetItemAsync<string>("token");

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        var expires = await localStorageService.GetItemAsync<long>("expires");

        var identity = expires > DateTime.UtcNow.Ticks
            ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "user") },
                authenticationType: nameof(CustomAuthenticationStateProvider))
            : new ClaimsIdentity();

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task Login(string token, long expiration)
    {
        await localStorageService.SetItemAsync("token", token);
        await localStorageService.SetItemAsync("expires", expiration);

        NotifyUserHasChanged();
    }

    public async Task Logout()
    {
        await localStorageService.RemoveItemAsync("token");

        NotifyUserHasChanged();
    }

    public void NotifyUserHasChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}