using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace blazor.wa.tbd.Infrastructure;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
    : AuthenticationStateProvider
{
    public ValueTask<string> Token => localStorageService.GetItemAsync<string>("token");

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorageService.GetItemAsync<string>("token");

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        var expires = await localStorageService.GetItemAsync<long>("expires");

        return expires > DateTime.UtcNow.Ticks
            ? new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.Name, "user") },
                authenticationType: nameof(CustomAuthenticationStateProvider))))
            : new AuthenticationState(new ClaimsPrincipal());
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