using flashcard.model.Entities;
using flashcard.utils;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Pages.Auth
{
	public partial class Profile : ComponentBase
	{
        [Inject]
        private Supabase.Client SupabaseClient { get; set; } = default!;

        private bool isAuthenticated = false;
		private string userName = string.Empty;
		private string userEmail = string.Empty;
		private List<Flashcard> flashCards = [];
		private List<Flashcard> filteredFlashCards = [];

		protected override async Task OnInitializedAsync()
		{

			var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			if (user.Identity?.IsAuthenticated == true)
			{
				flashCards = await FlashCardService.GetAllFlashCards();
				isAuthenticated = true;

				userEmail = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
						?? "No email";

				var userData = await SupabaseClient.From<SupabaseAccount>()
					.Where(x => x.Email == userEmail)
                    .Single();

                userName = userData!.Name;

				filteredFlashCards = flashCards
					.Where(fc => fc.AccountId == userData.Id)
					.ToList();
			}
		}
	}
}
