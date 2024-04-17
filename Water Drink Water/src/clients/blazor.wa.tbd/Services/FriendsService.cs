using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using viewmodels;

namespace blazor.wa.tbd.Services;

public class FriendsService(HttpClient client, AuthService authService)
{
    public async Task<bool> AddGroup(string name)
    {
        var token = await authService.TryGetAuthToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("api/groups",
            new { name });

        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<GroupViewModel>> GetGroups()
    {
        var token = await authService.TryGetAuthToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return Array.Empty<GroupViewModel>();
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("api/groups");

        if (!response.IsSuccessStatusCode)
        {
            return Array.Empty<GroupViewModel>();
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<GroupViewModel>>();
    }
}