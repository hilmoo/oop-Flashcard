using flashcard.model;
using flashcard.z_dummydata;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Pages
{
	public partial class Detail : ComponentBase
	{
		[Parameter]
		public string? Id { get; set; }

		private List<FlashCardProblem> soalll = [];
		private int currentIndex = 0;
		private bool IsStart { get; set; } = false;

		protected override void OnInitialized()
		{
			soalll = DummyDataCardBasic.GetFlashCardProblems();
		}

		private void HandleNext()
		{
			if (currentIndex < soalll.Count - 1)
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

		private bool CanNavigateNext => currentIndex < soalll.Count - 1;
		private bool CanNavigatePrev => currentIndex > 0;
	}
}
