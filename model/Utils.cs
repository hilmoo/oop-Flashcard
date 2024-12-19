namespace flashcard.model
{
    public class Category
    {
        public required string Value { get; init; }
        public required string DisplayName { get; init; }

        private static readonly List<Category> Categories =
        [
            new Category { Value = "all", DisplayName = "All Categories" },
            new Category { Value = "math", DisplayName = "Mathematics" },
            new Category { Value = "science", DisplayName = "Science" },
            new Category { Value = "programming", DisplayName = "Programming" },
            new Category { Value = "language", DisplayName = "Language" },
            new Category { Value = "history", DisplayName = "History" },
            new Category { Value = "geography", DisplayName = "Geography" },
            new Category { Value = "literature", DisplayName = "Literature" },
            new Category { Value = "art", DisplayName = "Art" },
            new Category { Value = "music", DisplayName = "Music" },
            new Category { Value = "physics", DisplayName = "Physics" },
            new Category { Value = "chemistry", DisplayName = "Chemistry" },
            new Category { Value = "biology", DisplayName = "Biology" },
            new Category { Value = "economics", DisplayName = "Economics" },
            new Category { Value = "philosophy", DisplayName = "Philosophy" }
        ];


        public static List<Category> GetCategories() => [.. Categories];

        public static List<Category> GetCategoriesExceptAll() =>
            Categories.Where(c => c.Value != "all").ToList();
    }
}