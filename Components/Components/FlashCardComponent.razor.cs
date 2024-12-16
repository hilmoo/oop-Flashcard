//using flashcard.model;
using flashcard.model.Entities;
using flashcard.utils;
using Microsoft.AspNetCore.Components;

namespace flashcard.Components.Components
{
	public partial class FlashCardComponent : ComponentBase
	{
		[Parameter]
		public Flashcard? flashCard { get; set; }
	}
}
