using BlazorSSO_JustWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// when setting up oidc it would be nice to get the host address here, then we can configure oidc redirects for all deployments
string clientBaseUrl = builder.HostEnvironment.BaseAddress;
if (clientBaseUrl.EndsWith("/")) { clientBaseUrl = clientBaseUrl.TrimEnd('/'); }
Console.Out.WriteLine("clientBaseUrl: " + clientBaseUrl);
builder.Services.AddOidcAuthentication(options => {
    builder.Configuration.Bind("GoogleSSO", options.ProviderOptions);
    options.ProviderOptions.ClientId = "xxx"; // we want to get this from an environment variable, but how??
    options.ProviderOptions.RedirectUri = clientBaseUrl + options.ProviderOptions.RedirectUri;
    options.ProviderOptions.PostLogoutRedirectUri = clientBaseUrl + options.ProviderOptions.PostLogoutRedirectUri;
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
