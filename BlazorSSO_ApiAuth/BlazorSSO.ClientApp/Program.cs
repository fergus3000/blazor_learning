using Blazored.SessionStorage;
using BlazorSSO.ClientApp;
using BlazorSSO.ClientApp.HttpClients;
using BlazorSSO.ClientApp.Providers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("GoogleSSO", options.ProviderOptions);
    // we also want to get the user's email address claim
    options.ProviderOptions.DefaultScopes.Add("email");
})
// now we also want to take extra steps to get a token from our api
.AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, CustomAccountFactory>(); ;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient<AppAuthClient>(options =>
{
    options.BaseAddress = new Uri("https://localhost:6001");
});
builder.Services.AddHttpClient<SecuredApiClient>(options =>
{
    options.BaseAddress = new Uri("https://localhost:6001");
});

// For convenience I use blazored.sessionstorage to access session storage (and the id token)
builder.Services.AddBlazoredSessionStorage();
// wireup my session storage provider
builder.Services.AddScoped<ISessionStorageProvider, SessionStorageProvider>();

await builder.Build().RunAsync();
