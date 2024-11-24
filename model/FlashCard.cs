namespace flashcard.model
{
	public class FlashCard
	{
		public required string Slug { get; set; }
		public required string Title { get; set; }
		public required string Description { get; set; }
		public required string Category { get; set; }
		public required int TotalQuestion { get; set; }
	}
	public class FlashCardDetail
	{
		public required string Question { get; set; }
		public required string Answer { get; set; }
		public int FlashCardId { get; set; }
	}
	public class FlashCardNew {
		public required string Title { get; set; }
		public required List<string> Questions { get; set; }
		public required List<string> Answers { get; set; }
		public required string Category { get; set; }
	}
	public class FlashCardProblem {
		public required string Question { get; set; }
		public required string Answer { get; set; }
	}
}
