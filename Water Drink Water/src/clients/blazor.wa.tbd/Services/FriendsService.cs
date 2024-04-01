using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using viewmodels;

namespace blazor.wa.tbd.Services;

public class FriendsService(HttpClient client, ILocalStorageService localStorage)
{
    private readonly Lazy<ValueTask<string>> _token = new(value: localStorage.GetItemAsync<string>("token"));
    private ValueTask<string> Token => _token.Value;

    public async Task<bool> AddGroup(string name)
    {
        var token = await _token.Value;

        if (string.IsNullOrWhiteSpace(token))
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
        var token = await _token.Value;

        if (string.IsNullOrWhiteSpace(token))
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