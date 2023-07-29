using Blazored.SessionStorage;
using System.Text.Json.Serialization;

namespace BlazorSSO.ClientApp
{
    public class SessionStorageProvider : ISessionStorageProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private readonly IConfiguration _configuration;

        public SessionStorageProvider(ISessionStorageService sessionStorageService, IConfiguration configuration)
        {
            _sessionStorageService = sessionStorageService;
            _configuration = configuration;
        }

        public async Task<string> GetIdToken()
        {
            // of the form: "oidc.user:{authority}:{clientId}"
            var oidcGoogleAuthSessionStorageKey = $"oidc.user:{_configuration["GoogleSSO:Authority"]}:{_configuration["GoogleSSO:ClientId"]}";
            var oidcModel = await _sessionStorageService.GetItemAsync<GoogleOidcModel>(oidcGoogleAuthSessionStorageKey);
            return oidcModel.IdToken;
        }

        public async Task SetApiAuthToken(string apiName, string tokenValue)
        {
            var tokenKey = GetApiTokenKey(apiName);
            await _sessionStorageService.SetItemAsStringAsync(tokenKey, tokenValue);
        }

        public async Task<string?> GetApiAuthToken(string apiName)
        {
            var tokenKey = GetApiTokenKey(apiName);
            if (await _sessionStorageService.ContainKeyAsync(tokenKey))
            {
                return await _sessionStorageService.GetItemAsStringAsync(tokenKey);
            }
            return null;
        }

        private string GetApiTokenKey(string apiName)
        {
            var tokenKey = $"apiToken:{apiName}";
            return tokenKey;
        }
    }

    public class GoogleOidcModel
    {
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
