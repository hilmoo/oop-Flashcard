using flashcard.model.Entities;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Components
{
	public partial class FlashCardComponent : ComponentBase
	{
        [Inject]
        private Supabase.Client _supabaseClient { get; set; } = default!;

        [Parameter]
		public Flashcard? flashCard { get; set; }

        private string? deckAuthor;

        protected override async Task OnParametersSetAsync()
        {
            var data = await _supabaseClient.From<SupabaseAccount>()
				.Where(x => x.Id == flashCard!.AccountId)
				.Single();
			deckAuthor = data!.Name;
        }
    }
}
