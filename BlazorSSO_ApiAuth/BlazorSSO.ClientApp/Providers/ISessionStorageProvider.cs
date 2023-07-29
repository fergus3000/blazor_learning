namespace BlazorSSO.ClientApp
{
    public interface ISessionStorageProvider
    {
        Task<string?> GetApiAuthToken(string apiName);
        Task<string> GetIdToken();
        Task SetApiAuthToken(string apiName, string tokenValue);
    }
}
