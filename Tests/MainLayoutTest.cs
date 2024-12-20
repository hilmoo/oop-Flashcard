using System.Security.Claims;
using flashcard.Components.Components;
using flashcard.Components.Layout;
using flashcard.Components.Pages;
using flashcard.Data;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Tests;

public class MainLayoutTest : TestContext
{
    [Fact]
    private void Render_AvatarComponent_WhenAuthorized()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("TEST USER");
        authContext.SetClaims(
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.DateOfBirth, "01-01-1970")
        );
        Services.AddDbContextFactory<DataContext>(options => { options.UseInMemoryDatabase("FakeDatabase"); });
        Services.AddSingleton<FlashCardService>();
        Services.AddSingleton<AccountServices>();

        var mainLayout = RenderComponent<MainLayout>();
        var avatarComponent = mainLayout.FindComponent<AvatarComponent>();

        Assert.NotNull(avatarComponent);
    }

    [Fact]
    private void Render_NameAvatarComponent_SameAsClaims()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("TEST USER");
        authContext.SetClaims(
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Name, "TEST USER")
        );
        Services.AddDbContextFactory<DataContext>(options => { options.UseInMemoryDatabase("FakeDatabase"); });
        Services.AddSingleton<FlashCardService>();
        Services.AddSingleton<AccountServices>();

        var mainLayout = RenderComponent<MainLayout>();
        var avatarComponent = mainLayout.FindComponent<AvatarComponent>();
        var liElement = avatarComponent.Find("li.text-sm.font-medium.text-gray-100");

        Assert.Equal("Hi, TEST USER", liElement.TextContent.Trim());
    }
}