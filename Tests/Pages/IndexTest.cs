using System.Security.Claims;
using flashcard.Components.Components;
using flashcard.Tests.Mock;
using Microsoft.AspNetCore.Components.Authorization;
using Index = flashcard.Components.Pages.Index;

namespace flashcard.Tests.Pages;

public class IndexTest : TestContext
{
    [Fact]
    public void Render_SearchBar_ReturnsTrue()
    {
        var authState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Email, "testuser@example.com")
        ], "mock")));
        AuthState.SetupAuthenticationState(Services);

        var authStateProvider = new AuthState.MockAuthenticationStateProvider(authState);
        Services.AddSingleton<AuthenticationStateProvider>(authStateProvider);

        var indexComponent = RenderComponent<Index>();
        var searchBar = indexComponent.Find("input[type='text'][placeholder='Search']");

        Assert.NotNull(searchBar);
    }

    [Fact]
    public void FlashCardGrid_DisplaysCorrectData()
    {
        AuthState.SetupAuthenticationState(Services);
        var flashCardsData = DummyDataTest.CardData.GetFlashCards();

        var component = RenderComponent<Index>(p => p.AddCascadingValue((flashCardsData)));
        var flashCardComponents = component.FindComponents<FlashCardComponent>();

        Assert.Equal(flashCardsData.Count, flashCardComponents.Count);
        foreach (var element in flashCardsData.Select(flashCard => component.Find(
                     $"div:contains('{flashCard.Title}')")))
        {
            Assert.NotNull(element);
        }
    }
}