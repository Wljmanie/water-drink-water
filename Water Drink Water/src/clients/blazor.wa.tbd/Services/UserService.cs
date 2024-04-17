using System.Net.Http.Headers;
using System.Net.Http.Json;
using viewmodels;

namespace blazor.wa.tbd.Services;

public class UserService(
    HttpClient client,
    AuthService authService)
{
    public async Task<bool> LogConsumption(int fluidOuncesConsumed)
    {
        var token = await authService.TryGetAuthToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("api/consumption",
            new { fluidOuncesConsumed });

        return response.IsSuccessStatusCode;
    }

    public async Task<int> GetConsumptionPercentage()
    {
        var token = await authService.TryGetAuthToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return 0;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        try
        {
            var response = await client.GetAsync("api/consumption");

            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            return await response.Content.ReadFromJsonAsync<int>();
        }
        catch (HttpRequestException ex)
        {
            return 0;
        }
    }

    public async Task<PreferencesViewModel?> GetPreferences()
    {
        var token = await authService.TryGetAuthToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.GetFromJsonAsync<PreferencesViewModel>("api/preferences");
    }

    public async Task<IEnumerable<TimeZoneModel>?> GetTimeZones()
    {
        return await client.GetFromJsonAsync<IEnumerable<TimeZoneModel>>("api/timezones");
    }

    public async Task<bool> SavePreferences(int targetFluidOunces, string timeZoneId)
    {
        var token = await authService.TryGetAuthToken();
        
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("api/preferences",
            new { targetFluidOunces, timeZoneId });

        return response.IsSuccessStatusCode;
    }

    public class TimeZoneModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }
}