using flashcard.model.Entities;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Components
{
    public partial class FlashCardComponent : ComponentBase
    {
        [Parameter] public Deck? DeckData { get; set; }
    }
}