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

        public static List<FlashCardProblem> GetFlashCardProblems()
        {
            return
            [
                new FlashCardProblem
                {
                    Question = "What is the output of the following code?",
                    Answer = "Hello World",
                },
                new FlashCardProblem
                {
                    Question = "What is",
                    Answer = "Hello World",
                },
                new FlashCardProblem
                {
                    Question = "What is",
                    Answer = "Hello World",
                },
                new FlashCardProblem
                {
                    Question = "Explain the difference between a class and an object in C#.",
                    Answer = "A class is a blueprint for creating objects, while an object is an instance of a class.",
                },
                new FlashCardProblem
                {
                    Question = "What is polymorphism in C#?",
                    Answer =
                        "Polymorphism is the ability of a method to do different things based on the object it is acting upon.",
                },
                new FlashCardProblem
                {
                    Question = "What is inheritance in C#?",
                    Answer =
                        "Inheritance is a feature that allows a class to inherit methods and properties from another class.",
                },
                new FlashCardProblem
                {
                    Question = "What is encapsulation in C#?",
                    Answer =
                        "Encapsulation is the concept of wrapping data and methods into a single unit called a class.",
                },
                new FlashCardProblem
                {
                    Question = "What is an interface in C#?",
                    Answer =
                        "An interface is a contract that defines a set of methods and properties that a class must implement.",
                },
                new FlashCardProblem
                {
                    Question = "What is the difference between an abstract class and an interface in C#?",
                    Answer =
                        "An abstract class can have implementations for some of its members, but an interface cannot have any implementation.",
                },
                new FlashCardProblem
                {
                    Question = "What is a delegate in C#?",
                    Answer =
                        "A delegate is a type that represents references to methods with a specific parameter list and return type.",
                },
                new FlashCardProblem
                {
                    Question = "What is the purpose of the 'using' statement in C#?",
                    Answer =
                        "The 'using' statement is used to ensure that IDisposable objects are properly disposed of.",
                },
                new FlashCardProblem
                {
                    Question = "What is the difference between 'ref' and 'out' parameters in C#?",
                    Answer =
                        "'ref' parameters must be initialized before being passed, while 'out' parameters must be assigned before the method returns.",
                }
            ];

        }
    }
}