using BlazorSSO.WebApi;
using BlazorSSO.WebApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Setup Token Settings from config
var configuration = builder.Configuration;
builder.Services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
builder.Services.Configure<GoogleTokenValidationSettings>(configuration.GetSection("GoogleIdTokenValidation"));

// wireup account logic
builder.Services.AddScoped<IUserAuth, UserAuth>();
// add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "corsService",
      builder =>
      {
          builder.AllowAnyOrigin();
          builder.AllowAnyHeader();
          builder.AllowAnyMethod();
      });
});

// setup authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var tokenSettings = configuration.GetSection("JwtTokenSettings").Get<JwtTokenSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = tokenSettings.Issuer,
        ValidateIssuer = true,
        ValidAudience = tokenSettings.Audience,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Key)),
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication(); // authentication must go before authorization here..
app.UseAuthorization();

app.MapControllers();

// more CORS
app.UseCors("corsService");

app.Run();
