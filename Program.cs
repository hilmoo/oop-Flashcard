using System.Globalization;
using System.Security.Claims;
using flashcard.Components;
using flashcard.Data;
using flashcard.utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Entities_Account = flashcard.model.Entities.Account;
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
services.AddControllers();

services.AddDbContextFactory<DataContext>(options => { options.UseNpgsql(PostgresConstants.ConnectionString); });
services.AddScoped<FlashCardService>();
services.AddScoped<AccountServices>();

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
            var tokens = ctx.Properties.GetTokens().ToList();

            tokens.Add(new AuthenticationToken
            {
                Name = "TicketCreated",
                Value = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            });

            ctx.Properties.StoreTokens(tokens);

            if (ctx.Principal == null)
            {
                return Task.CompletedTask; // TODO: add handler
            }

            var email = ctx.Principal.FindFirstValue(ClaimTypes.Email);
            var name = ctx.Principal.FindFirstValue(ClaimTypes.Name);
            var googleId = ctx.Principal.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validate essential claims
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException("Email claim is missing or empty.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Name claim is missing or empty.");
            }

            if (string.IsNullOrWhiteSpace(googleId))
            {
                throw new InvalidOperationException("GoogleId claim is missing or empty.");
            }

            // Create a new account object
            var newAccount = new Entities_Account
            {
                Email = email,
                Name = name,
                GoogleId = googleId
            };

            var dbAccountServices = ctx.HttpContext.RequestServices.GetService<AccountServices>();
            if (dbAccountServices == null)
            {
                throw new InvalidOperationException("Unable to resolve DbAccountServices from DI container.");
            }

            dbAccountServices.AddNewAccount(newAccount);
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

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();