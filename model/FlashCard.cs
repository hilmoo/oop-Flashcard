namespace flashcard.model
{
    public class DeckBase
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required int TotalQuestion { get; set; }
        public required bool IsPublic { get; set; }
        public required string GoogleId { get; set; }
    }

    public class DeckBasic : DeckBase
    {
        public required string Slug { get; set; }
    }

    public class FlashCardProblem
    {
        public required string Question { get; set; }
        public required string Answer { get; set; }
    }
}