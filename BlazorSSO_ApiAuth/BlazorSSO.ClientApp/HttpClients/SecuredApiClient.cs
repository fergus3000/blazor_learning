using static BlazorSSO.ClientApp.Pages.FetchData;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlazorSSO.ClientApp.HttpClients
{
    public class SecuredApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageProvider _sessionStorageProvider;

        public SecuredApiClient(HttpClient httpClient, ISessionStorageProvider sessionStorageProvider)
        {
            _httpClient = httpClient;
            _sessionStorageProvider = sessionStorageProvider;
        }

        public async Task<WeatherForecast[]> GetWeatherForecast()
        {
            var mediaType = new MediaTypeHeaderValue("application/json");
            var authToken = await _sessionStorageProvider.GetApiAuthToken("SecuredApiClient");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);
            var response = await _httpClient.GetAsync("/weatherforecast");
            response.EnsureSuccessStatusCode();
            var forecast = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
            return forecast;
        }
    }
}