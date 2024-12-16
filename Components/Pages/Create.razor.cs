using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using flashcard.utils;
using flashcard.model.Entities;

namespace flashcard.Components.Pages
{
	public partial class Create : ComponentBase
	{
		[Inject]
		private Supabase.Client _supabaseClient { get; set; } = default!;

		[CascadingParameter]
        private Task<AuthenticationState>? AuthenticationState { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

		//[Inject]
		//private FlashCardService StateService { get; set; }

		private string deckName
		{
			get => FlashCardService.DeckName;
			set => FlashCardService.DeckName = value;
		}

		private string selectedCategory
		{
			get => FlashCardService.SelectedCategory;
			set => FlashCardService.SelectedCategory = value;
		}

		private string deckVisibility = "Public";

		private bool isSubmitting = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationState!;
            if (!authState.User.Identity!.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/auth/signin");
                return;
            }
        }

        //private string deckName = string.Empty;
		//private void HandleDeckName(ChangeEventArgs e)
		//{
		//	deckName = e.Value?.ToString() ?? string.Empty;
		//}
		//private string selectedCategory = string.Empty;
		//private void HandleSelectCategory(ChangeEventArgs e)
		//{
		//	selectedCategory = e.Value?.ToString() ?? string.Empty;
		//}

		private string tempQuestion = string.Empty;
		private string tempAnswer = string.Empty;
		private void HandleQuestionChange(ChangeEventArgs e)
		{
			tempQuestion = e.Value?.ToString() ?? string.Empty;
		}

		private void HandleAnswerChange(ChangeEventArgs e)
		{
			tempAnswer = e.Value?.ToString() ?? string.Empty;
		}

		private void HandleDiscard()
		{
			tempQuestion = string.Empty;
			tempAnswer = string.Empty;
		}

		private void HandleSave()
		{
			if (!string.IsNullOrWhiteSpace(tempQuestion) && !string.IsNullOrWhiteSpace(tempAnswer))
			{
				AddFlashCardProblem(tempQuestion, tempAnswer);
				tempQuestion = string.Empty;
				tempAnswer = string.Empty;

			}
		}
		private void AddFlashCardProblem(string question, string answer)
		{
			FlashCardService.AddFlashCardProblem(question, answer);
		}

		private void DeleteFlashCardProblem(int Index)
		{
			FlashCardService.DeleteFlashCardProblem(Index);
		}
        private async Task HandleFinalize()
        {
            if (string.IsNullOrWhiteSpace(deckName) || string.IsNullOrWhiteSpace(selectedCategory))
            {
                // Show error message
                return;
            }

            if (FlashCardService.FlashCardProblems.Count == 0)
            {
                // Show error message
                return;
            }

            isSubmitting = true;

			try
            {
				var authState = await AuthenticationState!;
				var user = authState.User;
				string userEmail = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? "No email";

				Console.WriteLine(userEmail);
				if (string.IsNullOrEmpty(userEmail))
					throw new Exception("User email is not available.");

				var accountData = await _supabaseClient.From<SupabaseAccount>()
                    .Where(x => x.Email == userEmail)
                    .Single();

                Console.WriteLine("Account ID: " + accountData!.Id);
				Console.WriteLine("Deck Visibility: " + deckVisibility);

				await FlashCardService.SaveDeckToSupabase(deckName, selectedCategory, accountData!.Id, deckVisibility);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                // Handle error
                Console.WriteLine(ex);
            }
            finally
            {
                isSubmitting = false;
            }
        }
    }
}
