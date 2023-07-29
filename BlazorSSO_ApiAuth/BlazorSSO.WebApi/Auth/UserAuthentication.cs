using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BlazorSSO.Shared;

namespace BlazorSSO.WebApi.Auth
{
    public class UserAuth : IUserAuth
    {
        private readonly JwtTokenSettings _tokenSettings;

        public UserAuth(IOptions<JwtTokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<(bool IsAuthorized, TokenResponseModel Token)> AuthorizeGoogleUser(string userEmail)
        {
            var userRoles = new List<UserRoles>();
            if (!UserEmailIsAllowed(userEmail))
                return (false, null);
            var token = new TokenResponseModel
            {
                JwtToken = GenerateJwt(userRoles)
            };
            return (true, token);
        }

        private bool UserEmailIsAllowed(string email)
        {
            // for this iteration, we just want to show that everything fits together
            return true;
        }

        private string GenerateJwt(List<UserRoles> roles)
        {
            var securtityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));

            var credentials = new SigningCredentials(securtityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            if ((roles?.Count ?? 0) > 0)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
