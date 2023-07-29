using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using BlazorSSO.ClientApp.HttpClients;
using BlazorSSO.Shared;

namespace BlazorSSO.ClientApp.Providers
{
    public class CustomAccountFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly AppAuthClient _appAuthClient;
        private readonly ISessionStorageProvider _googleOidcProvider;

        public CustomAccountFactory(IAccessTokenProviderAccessor accessor, AppAuthClient appAuthClient, ISessionStorageProvider googleOidcProvider) : base(accessor)
        {
            _appAuthClient = appAuthClient;
            _googleOidcProvider = googleOidcProvider;
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);
            try
            {
                if (initialUser.Identity.IsAuthenticated)
                {
                    var idToken = await _googleOidcProvider.GetIdToken();
                    var authRequest = new AuthenticationRequestModel { IdToken = idToken };
                    var response = await _appAuthClient.AuthenticateGoogleUser(authRequest);
                    ((ClaimsIdentity)initialUser.Identity).AddClaim(new Claim("APIjwt", response.JwtToken));
                    await _googleOidcProvider.SetApiAuthToken("SecuredApiClient", response.JwtToken);
                }
            }
            catch
            {
                initialUser = new ClaimsPrincipal(new ClaimsIdentity());
            }

            return initialUser;
        }
    }
}
