namespace flashcard.model;

public class Category
{
    public required string Value { get; init; }
    public required string DisplayName { get; init; }

    private static readonly List<Category> Categories =
    [
        new() { Value = "all", DisplayName = "All Categories" },
        new() { Value = "art", DisplayName = "Art" },
        new() { Value = "biology", DisplayName = "Biology" },
        new() { Value = "chemistry", DisplayName = "Chemistry" },
        new() { Value = "economics", DisplayName = "Economics" },
        new() { Value = "geography", DisplayName = "Geography" },
        new() { Value = "history", DisplayName = "History" },
        new() { Value = "language", DisplayName = "Language" },
        new() { Value = "literature", DisplayName = "Literature" },
        new() { Value = "math", DisplayName = "Mathematics" },
        new() { Value = "music", DisplayName = "Music" },
        new() { Value = "philosophy", DisplayName = "Philosophy" },
        new() { Value = "physics", DisplayName = "Physics" },
        new() { Value = "programming", DisplayName = "Programming" },
        new() { Value = "science", DisplayName = "Science" }
    ];


    public static List<Category> GetCategories() => [.. Categories];

    public static List<Category> GetCategoriesExceptAll() =>
        Categories.Where(c => c.Value != "all").ToList();
}