namespace flashcard.model
{
    public class Category
    {
        public required string Value { get; set; }
        public required string DisplayName { get; set; }
        private static readonly List<Category> _categories =
        [
            new Category { Value = "all", DisplayName = "All Categories" },
            new Category { Value = "math", DisplayName = "Mathematics" },
            new Category { Value = "science", DisplayName = "Science" },
            new Category { Value = "programming", DisplayName = "Programming" },
            new Category { Value = "bookmark", DisplayName = "Bookmark" },
            new Category { Value = "own", DisplayName = "My Deck" }
        ];

        public static List<Category> GetCategories() => [.. _categories];

        public static List<Category> GetCategoriesExceptAll() =>
        _categories.Where(c => c.Value != "all").ToList();
    }
}