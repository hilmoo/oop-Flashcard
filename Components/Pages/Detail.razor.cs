using flashcard.model.Entities;
using flashcard.utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace flashcard.Components.Pages
{
	public partial class Detail : ComponentBase
	{
		[Parameter]
		public string? Slug { get; set; }

		private bool isFirstRender = true;

		private List<Problem> soal = [];
		private Flashcard? deckData;
		private string? author;
        private int currentIndex = 0;
		private bool IsStart { get; set; } = false;

		protected override async Task OnInitializedAsync()
		{
			soal = await FlashCardService.GetProblemsByFlashcardSlug(Slug);
			deckData = await FlashCardService.GetFlashcardBySlug(Slug);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await LoadSavedState();
				isFirstRender = false;
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

		private class SavedState
		{
			public string? Slug { get; set; }
			public int CurrentIndex { get; set; }
		}

		private async Task LoadSavedState()
		{
			try
			{
				var savedState = await JSRuntime.InvokeAsync<string>("localStorage.getItem", $"flashcard_state_{Slug}");
				if (!string.IsNullOrEmpty(savedState))
				{
					var state = JsonSerializer.Deserialize<SavedState>(savedState);
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
			await JSRuntime.InvokeVoidAsync("localStorage.setItem", $"flashcard_state_{Slug}", JsonSerializer.Serialize(state));
		}

		private void ToggleStart() => IsStart = !IsStart;

		private bool CanNavigateNext => currentIndex < soal.Count - 1;
		private bool CanNavigatePrev => currentIndex > 0;
	}
}
