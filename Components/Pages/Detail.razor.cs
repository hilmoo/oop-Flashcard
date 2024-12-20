using flashcard.model.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Claims;
using static System.Text.Json.JsonSerializer;

namespace flashcard.Components.Pages;

public partial class Detail : ComponentBase
{
    [Parameter] public required string Slug { get; set; }

    private List<FlashCard> soal = [];
    private Deck? deckData;
    private int currentIndex;
    private bool canEdit;
    private bool IsStart { get; set; }
    private string? userEmail;
    private bool isMarked;
    private bool isAuthenticated;
    private string CurrentProgress => $"{(IsStart ? currentIndex + 1 : currentIndex)} / {soal.Count}";

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
        isAuthenticated = user.Identity?.IsAuthenticated ?? false;

        var canSee = await FlashCardService.IsCanSeeDeck(userEmail!, Slug);
        if (!canSee)
        {
            Navigation.NavigateTo("/");
            return;
        }

        if (!string.IsNullOrEmpty(userEmail))
        {
            canEdit = await FlashCardService.IsCanEditDeck(userEmail!, Slug);
            isMarked = await FlashCardService.IsDeckMarked(userEmail!, Slug);
        }

        soal = await FlashCardService.GetFlashcardByDeckSlug(Slug);
        deckData = await FlashCardService.GetDeckBySlug(Slug);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadSavedState();
            StateHasChanged();
        }
    }

    private async Task HandleNext()
    {
        if (currentIndex < soal.Count - 1)
        {
            currentIndex++;
            await SaveStateToLocalStorage();
        }
    }

    private async Task HandlePrev()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            await SaveStateToLocalStorage();
        }
    }

    private async Task HandleDeckMark()
    {
        await FlashCardService.SetDeckMark(userEmail!, Slug);
        isMarked = true;
        StateHasChanged();
    }

    private async Task HandleDeckUnmark()
    {
        await FlashCardService.RemoveDeckMark(userEmail!, Slug);
        isMarked = false;
        StateHasChanged();
    }

    private class SavedState
    {
        public string? Slug { get; init; }
        public int CurrentIndex { get; init; }
    }

    private async Task LoadSavedState()
    {
        try
        {
            var savedState = await JsRuntime.InvokeAsync<string>("localStorage.getItem", $"flashcard_state_{Slug}");
            if (!string.IsNullOrEmpty(savedState))
            {
                var state = Deserialize<SavedState>(savedState);
                if (state != null && state.Slug == Slug)
                {
                    currentIndex = state.CurrentIndex;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task SaveStateToLocalStorage()
    {
        var state = new SavedState
        {
            Slug = Slug,
            CurrentIndex = currentIndex
        };
        await JsRuntime.InvokeVoidAsync("localStorage.setItem", $"flashcard_state_{Slug}",
            Serialize(state));
    }

    private void ToggleStart() => IsStart = !IsStart;

    private bool CanNavigateNext => currentIndex < soal.Count - 1;
    private bool CanNavigatePrev => currentIndex > 0;
}