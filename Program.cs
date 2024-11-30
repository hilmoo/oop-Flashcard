using flashcard.Components;
using flashcard.Data;
using flashcard.services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<FlashcardDbContext>(options =>
{
	options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"));
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//  .AddCookie(options =>
//  {
//    options.Cookie.Name = "auth";
//    options.LoginPath = "/login";
//    options.Cookie.MaxAge = TimeSpan.FromDays(1);
//  });
//builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton<FlashCardService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
