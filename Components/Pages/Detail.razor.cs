using flashcard.model.Entities;
using flashcard.utils;
using flashcard.z_dummydata;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Pages
{
	public partial class Detail : ComponentBase
	{
		[Parameter]
		public string? Slug { get; set; }

		//private List<FlashCardProblem> soalll = [];
		private List<Problem> soal = [];
		private int currentIndex = 0;
		private bool IsStart { get; set; } = false;

		protected override async Task OnInitializedAsync()
		{
			soal = await FlashCardService.GetProblemsByFlashcardSlug(Slug);
		}

		private void HandleNext()
		{
			if (currentIndex < soal.Count - 1)
			{
				currentIndex++;
			}
		}

		private void HandlePrev()
		{
			if (currentIndex > 0)
			{
				currentIndex--;
			}
		}

		private void ToggleStart() => IsStart = !IsStart;

		private bool CanNavigateNext => currentIndex < soal.Count - 1;
		private bool CanNavigatePrev => currentIndex > 0;
	}
}
