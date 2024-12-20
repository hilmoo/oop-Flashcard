using flashcard.model.Entities;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace flashcard.Components.Pages;

public partial class Deckmark : ComponentBase
{
    private string searchText = string.Empty;
    private string selectedCategory = string.Empty;
    private List<Deck> deck = [];
    private List<Deck> filteredDeck = [];
    private string? userEmail;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated ?? false)
        {
            userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
            deck = await FlashCardService.GetMarkedDecks(userEmail!);
            filteredDeck = deck;
        }
        else
        {
            Navigation.NavigateTo("/auth/signin");
        }
    }

    private void ApplyFilters()
    {
        var searchTextLower = searchText.ToLowerInvariant();
        var selectedCategoryLower = selectedCategory.ToLowerInvariant();

        filteredDeck = deck
            .Where(fc =>
                (string.IsNullOrEmpty(searchText) ||
                 fc.Title!.Contains(searchTextLower, StringComparison.InvariantCultureIgnoreCase)) &&
                (selectedCategory == "all" || string.IsNullOrEmpty(selectedCategory) ||
                 fc.Category!.ToLowerInvariant().Equals(selectedCategoryLower)))
            .ToList();
    }

    private void HandleSearch(ChangeEventArgs e)
    {
        searchText = e.Value?.ToString() ?? string.Empty;
        ApplyFilters();
    }

    private void SelectCategory(ChangeEventArgs e)
    {
        selectedCategory = e.Value?.ToString() ?? string.Empty;
        ApplyFilters();
    }
}