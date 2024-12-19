using flashcard.model;

namespace flashcard.Tests.Mock;

public static class DummyDataTest
{
    public static class CardData
    {
        public static List<FlashCard> GetFlashCards()
        {
            return
            [
                new FlashCard
                {
                    Slug = "intro-to-csharp",
                    Title = "Introduction to C#",
                    Description =
                        "Learn the basics of C# programming language.aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    Category = "programming",
                    TotalQuestion = 10,
                },
                new FlashCard
                {
                    Slug = "advanced-csharp",
                    Title = "Advanced C#",
                    Description =
                        "Deep dive into advanced C# topics. Deep dive into advanced C# topics. Deep dive into advanced C# topics. Deep dive into advanced C# topics. Deep dive into advanced C# topics. Deep dive into advanced C# topics. Deep dive into advanced C# topics. Deep dive into advanced C# topics. Deep dive into advanced C# topics.",
                    Category = "math",
                    TotalQuestion = 10,
                },
                new FlashCard
                {
                    Slug = "csharp-linq",
                    Title = "C# LINQ",
                    Description = "Learn how to use LINQ in C#.",
                    Category = "science",
                    TotalQuestion = 10,
                }
            ];
        }
    }
}