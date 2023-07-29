using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using BlazorSSO.WebApi.Auth;
using BlazorSSO.Shared;
using BlazorSSO.WebApi;
using Microsoft.Extensions.Options;

namespace BlazorSSO.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserAuth _userAuth;
        private readonly GoogleTokenValidationSettings _tokenValidationSettings;
        public UserController(IUserAuth userAuth, IOptions<GoogleTokenValidationSettings> tokenValidationSettings)
        {
            _userAuth = userAuth;
            _tokenValidationSettings = tokenValidationSettings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateGoogleUser(AuthenticationRequestModel authRequest)
        {
            var idToken = authRequest.IdToken;
            var payload = await VerifyGoogleTokenId(idToken);
            if (payload == null)
            {
                return BadRequest("Invalid token");
            }
            else if (!payload.EmailVerified)
            {
                return Unauthorized("Unverified email");
            }
            var result = await _userAuth.AuthorizeGoogleUser(payload.Email);
            if (result.IsAuthorized)
            {
                return Ok(result.Token);
            }
            return Unauthorized($"User {payload.Email} is not authorized");
            
        }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenId(string token)
        {
            try
            {
                // must add extra validation settings if we want to ensure the token has the correct audience
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new string[] { _tokenValidationSettings.Audience }
                };
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

                return payload;
            }
            catch (System.Exception)
            {
                Console.WriteLine("invalid google token");

            }
            return null;
        }
    }
}
