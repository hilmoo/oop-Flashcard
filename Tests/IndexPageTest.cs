using System.Security.Claims;
using flashcard.Components.Components;
using flashcard.Components.Layout;
using flashcard.Data;
using Microsoft.EntityFrameworkCore;
using Index = flashcard.Components.Pages.Index;

namespace flashcard.Tests;

public class IndexPageTest : TestContext
{
    [Fact]
    private void Render_SearchBar_ReturnsTrue()
    {
        this.AddTestAuthorization();
        Services.AddDbContextFactory<DataContext>(options => { options.UseInMemoryDatabase("FakeDatabase"); });
        Services.AddSingleton<FlashCardService>();
        Services.AddSingleton<AccountServices>();

        var indexComponent = RenderComponent<Index>();
        var searchBar = indexComponent.Find("input[type='text'][placeholder='Search']");

        Assert.NotNull(searchBar);
    }

    [Fact]
    private void Render_SelectCategory_ReturnsTrue()
    {
        this.AddTestAuthorization();
        Services.AddDbContextFactory<DataContext>(options => { options.UseInMemoryDatabase("FakeDatabase"); });
        Services.AddSingleton<FlashCardService>();
        Services.AddSingleton<AccountServices>();

        var indexComponent = RenderComponent<Index>();
        var selectElement = indexComponent.Find("select");
        var selectedOption = selectElement.QuerySelector("option[selected]");

        Assert.NotNull(selectedOption);
        Assert.Equal("Select Category", selectedOption.TextContent);
    }

    [Fact]
    private void Render_Profile_ReturnsName()
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
}