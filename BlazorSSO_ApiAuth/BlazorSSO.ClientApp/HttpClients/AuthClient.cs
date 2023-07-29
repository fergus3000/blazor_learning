using BlazorSSO.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace BlazorSSO.ClientApp.HttpClients
{
    public class AppAuthClient
    {
        private HttpClient _httpClient;
        public AppAuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // this method sends the google id token to our api, which will validate against google, and return a jwt listing entitlements
        public async Task<TokenResponseModel> AuthenticateGoogleUser(AuthenticationRequestModel authenticationRequest)
        {
            var mediaType = new MediaTypeHeaderValue("application/json");
            var postData = new StringContent(JsonSerializer.Serialize(authenticationRequest), Encoding.UTF8, mediaType);
            var response = await _httpClient.PostAsync("/user/authenticate", postData);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TokenResponseModel>();
        }
    }
}
