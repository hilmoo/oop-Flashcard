namespace flashcard.model
{
	using System.ComponentModel.DataAnnotations;

	public class FlashCardBasic
	{
		[Key]
		public required int Id { get; set; }
		public required string Slug { get; set; }
		public required string Title { get; set; }
		public required int CurrentProgress { get; set; }
		public required int TotalProgress { get; set; }
	}
}
