using System.Security.Claims;
using flashcard.Components;
using flashcard.Data;
using flashcard.utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using DotEnv = SimpleDotEnv.DotEnv;

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);
var logging = builder.Logging;
var services = builder.Services;

logging.ClearProviders();
logging.AddConsole();

services.AddHttpContextAccessor();
services.AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddDbContextFactory<DataContext>(options => { options.UseNpgsql(PostgresConstants.ConnectionString); });
services.AddSingleton<FlashCardService>();
services.AddTransient<DbAccountServices>();

services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.Cookie.Name = AuthenticationConstants.CookieName;
        options.Cookie.MaxAge = TimeSpan.FromDays(1);
    }).AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ??
                                 throw new InvalidOperationException(
                                     "GOOGLE_CLIENT_ID is not set in the environment variables.");
        googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ??
                                     throw new InvalidOperationException(
                                         "GOOGLE_CLIENT_SECRET is not set in the environment variables.");
        googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        googleOptions.SaveTokens = true;
        googleOptions.Events.OnCreatingTicket = ctx =>
        {
            List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

            tokens.Add(new AuthenticationToken()
            {
                Name = "TicketCreated",
                Value = DateTime.UtcNow.ToString()
            });

            ctx.Properties.StoreTokens(tokens);

            if (ctx.Principal == null)
            {
                return Task.CompletedTask; // TODO: add handler
            }

            var email = ctx.Principal.FindFirstValue(ClaimTypes.Email);
            var name = ctx.Principal.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
            {
                throw new InvalidOperationException("The email or name cannot be null or empty.");
            }

            var dbAccountServices = ctx.HttpContext.RequestServices.GetService<DbAccountServices>();
            if (dbAccountServices == null)
            {
                throw new InvalidOperationException("Unable to resolve DbAccountServices from DI container.");
            }

            dbAccountServices.AddNewAccount(email, name);
            return Task.CompletedTask;
        };
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();