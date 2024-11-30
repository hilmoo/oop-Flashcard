using flashcard.model;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Components
{
	public partial class FlashCardComponent : ComponentBase
	{
		[Parameter]
		public FlashCard? flashCard { get; set; }
	}
}
