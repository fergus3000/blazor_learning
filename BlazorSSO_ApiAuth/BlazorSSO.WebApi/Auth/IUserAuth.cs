using BlazorSSO.Shared;

namespace BlazorSSO.WebApi.Auth
{
    public interface IUserAuth
    {
        Task<(bool IsAuthorized, TokenResponseModel Token)> AuthorizeGoogleUser(string userEmail);
    }
}