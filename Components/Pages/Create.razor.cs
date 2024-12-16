using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace flashcard.Components.Pages
{
	public partial class Create : ComponentBase
	{
		[Inject]
		public required NavigationManager NavigationManager { get; set; }

		private AuthenticationState? _authenticationState;

		protected override async Task OnInitializedAsync()
		{
			_authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var user = _authenticationState.User;

			if (user.Identity == null || !user.Identity.IsAuthenticated)
			{
				NavigationManager.NavigateTo("/auth/signin");
			}
		}

		private string deckName = string.Empty;
		private void HandleDeckName(ChangeEventArgs e)
		{
			deckName = e.Value?.ToString() ?? string.Empty;
		}
		private string selectedCategory = string.Empty;
		private void HandleSelectCategory(ChangeEventArgs e)
		{
			selectedCategory = e.Value?.ToString() ?? string.Empty;
		}

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
	}
}
