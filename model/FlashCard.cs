namespace flashcard.model;

public class DeckBase
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required int TotalQuestion { get; init; }
    public required bool IsPublic { get; init; }
    public required string GoogleId { get; init; }
}

public class FlashCardProblem
{
    public required string Question { get; init; }
    public required string Answer { get; init; }
}