using flashcard.model.Entities;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Components;

public partial class FlashCardComponent : ComponentBase
{
    private string? author;
    [Parameter] public Deck? DeckData { get; set; }

    protected override async Task OnInitializedAsync()
    {
        author = await FlashCardService.GetAuthorByAccountId(DeckData!.AccountId);
    }
}