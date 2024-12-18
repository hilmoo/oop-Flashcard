using Bunit;
using Xunit;
using Microsoft.AspNetCore.Components.Authorization;
using flashcard.model;
using flashcard.model.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;


namespace FlashcardTests
{
    public class FlashcardSearchTests : TestContext
    {
        [Fact]
        public void SearchInput_UpdatesFilteredFlashCards()
        {

            // Mock AuthenticationStateProvider
            var fakeAuthState = new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "testuser@example.com")
                }, "FakeAuthType")
            ));

            var categories = new List<Category>
            {
                new Category { Value = "math", DisplayName = "Math" },
                new Category { Value = "science", DisplayName = "Science" }
            };

            var flashcards = new List<FlashCard>
            {
                new FlashCard { Title = "Algebra", Category = "math", Slug="aaaa", Description = "bbbb", TotalQuestion = 9},
                new FlashCard { Title = "Biology", Category = "science", Slug="aaaa", Description = "bbbb", TotalQuestion = 9},
                new FlashCard { Title = "Geometry", Category = "math", Slug="aaaa", Description = "bbbb", TotalQuestion = 9}
            };

            Services.AddAuthorization(); // Add default Authorization
            Services.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider(fakeAuthState));
            var component = RenderComponent<flashcard.Components.Pages.Index>(); // Replace with your page/component name


            // Act
            var searchInput = component.Find("input[type='text']");
            searchInput.Input("Algebra"); // Simulate typing "Algebra" into the search bar

            // Assert
            var debugSearch = component.Markup.Contains("Debug search: Algebra"); // Check if search text updates
            Assert.True(debugSearch);

        }
    }

    public class FlashcardSearch2Tests : TestContext
    {
        [Fact]
        public void SearchInput_FiltersFlashcardsBasedOnSearchText()
        {
            // Arrange
            var flashcards = new List<FlashCard>
            {
                new FlashCard { Title = "Algebra", Category = "math", Slug="aaaa", Description = "bbbb", TotalQuestion = 9},
                new FlashCard { Title = "Biology", Category = "science", Slug="aaaa", Description = "bbbb", TotalQuestion = 9},
                new FlashCard { Title = "Geometry", Category = "math", Slug="aaaa", Description = "bbbb", TotalQuestion = 9},
                new FlashCard { Title = "Chemistry", Category = "science", Slug="aaaa", Description = "bbbb", TotalQuestion = 9}
            };

            // Mock AuthenticationStateProvider
            var fakeAuthState = new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "testuser@example.com")
                }, "FakeAuth")));

            Services.AddAuthorization();
            Services.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider(fakeAuthState));

            // Mock FlashCardService untuk menyediakan data flashcards
            Services.AddSingleton<FlashCardService>(new FlashCardService(flashcards));

            // Render komponen Flashcard
            var component = RenderComponent<flashcard.Components.Pages.Index>();
    
            // Act
            var searchInput = component.Find("input.grow");

            // Simulasikan input pencarian "Algebra"
            searchInput.Input("Algebra"); // Trigger the change event with the search text
            
            // Assert
            // Periksa bahwa hanya flashcard "Algebra" yang ditampilkan
            var renderedFlashCards = component.FindAll("flashcardcomponent");
            Assert.Single(renderedFlashCards); // Hanya satu flashcard yang tampil
            Assert.Contains("Algebra", renderedFlashCards[0].TextContent); // Pastikan "Algebra" ditampilkan
        }
    }

    public class FlashcardDropdownTests : TestContext
    {
        [Fact]
        public void Dropdown_Selection_UpdatesDebugCategoryAndFiltersFlashcards()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Value = "math", DisplayName = "Math" },
                new Category { Value = "science", DisplayName = "Science" }
            };

            var flashcards = new List<FlashCard>
            {
                new FlashCard { Title = "Algebra", Category = "math", Slug="aaaa", Description = "bbbb", TotalQuestion = 9},
                new FlashCard { Title = "Biology", Category = "science", Slug="aaaa", Description = "bbbb", TotalQuestion = 9},
                new FlashCard { Title = "Geometry", Category = "math", Slug="aaaa", Description = "bbbb", TotalQuestion = 9}
            };

            // Mock AuthenticationStateProvider
            var fakeAuthState = new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "testuser@example.com")
                }, "FakeAuth")));

            Services.AddAuthorization();
            Services.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider(fakeAuthState));

            // Mock services for categories and flashcards
            Services.AddSingleton<CategoryService>(new CategoryService(categories));
            Services.AddSingleton<FlashCardService>(new FlashCardService(flashcards));

            // Render the Flashcard component
            var component = RenderComponent<flashcard.Components.Pages.Index>();

            // Act
            var dropdown = component.Find("select");
            dropdown.Change(new ChangeEventArgs { Value = "math" }); // Simulate selecting "math" from the dropdown

            // Assert
            // Check that debug category text is updated
            var debugCategoryText = component.Markup.Contains("debug category: math");
            Assert.True(debugCategoryText, "The debug category should display 'math'.");

            // Verify filtered flashcards
            var renderedFlashCards = component.FindAll(".flashcard-class");
            Assert.Equal(2, renderedFlashCards.Count); // There are 2 flashcards under the "math" category

        }
    }

    // Fake AuthenticationStateProvider
    public class FakeAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationState _authState;

        public FakeAuthenticationStateProvider(AuthenticationState authState)
        {
            _authState = authState;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(_authState);
        }
    }
    // Example of Mock Services
    public class CategoryService
    {
        private readonly List<Category> _categories;
        public CategoryService(List<Category> categories) => _categories = categories;
        public List<Category> GetCategories() => _categories;
    }

    public class FlashCardService
    {
        private readonly List<FlashCard> _flashCards;
        public FlashCardService(List<FlashCard> flashCards) => _flashCards = flashCards;
        public List<FlashCard> GetFlashCards() => _flashCards;
    }
}
