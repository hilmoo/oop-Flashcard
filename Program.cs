using flashcard.Components;
using flashcard.Data;
using flashcard.utils;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<DataContext>(options =>
{
	options.UseNpgsql(PostgresConstants.ConnectionString);
});


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddAuthentication("Cookies")
	.AddCookie(options =>
	{
		options.Cookie.Name = AuthenticationConstants.CookieName;
		options.Cookie.MaxAge = TimeSpan.FromDays(1);
	}).AddGoogle(googleOpt =>
	{
		googleOpt.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? throw new InvalidOperationException("GOOGLE_CLIENT_ID is not set in the environment variables.");
		googleOpt.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? throw new InvalidOperationException("GOOGLE_CLIENT_SECRET is not set in the environment variables.");
	});

builder.Services.AddSingleton<FlashCardService>();
builder.Services.AddTransient<DbAccountServices>();

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
