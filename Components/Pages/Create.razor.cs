using System.Security.Claims;
using flashcard.model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace flashcard.Components.Pages;

public partial class Create : ComponentBase
{
    private string? DeckName { get; set; }
    private string? SelectedCategory { get; set; }
    private string? DeckDescription { get; set; }
    private bool deckVisibility = true;
    private bool isSubmitting;
    private string? TempQuestion { get; set; }
    private string? TempAnswer { get; set; }
    private readonly List<FlashCardProblem> flashCardProblems = [];
    private string editingQuestion = string.Empty;
    private string editingAnswer = string.Empty;
    private int editingIndex = -1;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (!authState.User.Identity!.IsAuthenticated)
        {
            Navigation.NavigateTo("/auth/signin");
        }
    }

    private void HandleDiscard()
    {
        TempQuestion = string.Empty;
        TempAnswer = string.Empty;
    }

    private void HandleSave()
    {
        if (string.IsNullOrWhiteSpace(TempQuestion) || string.IsNullOrWhiteSpace(TempAnswer)) return;
        AddFlashCardProblem(TempQuestion, TempAnswer);
        Console.WriteLine("Flashcard problem added with question: " + TempQuestion + " and answer: " + TempAnswer);
        TempQuestion = string.Empty;
        TempAnswer = string.Empty;
    }

    private void AddFlashCardProblem(string question, string answer)
    {
        flashCardProblems.Add(new FlashCardProblem { Question = question, Answer = answer });
    }

    private void DeleteFlashCardProblem(int index)
    {
        flashCardProblems.RemoveAt(index);
    }

    private async Task OpenEditModal(int index)
    {
        editingIndex = index;
        var problem = flashCardProblems[index];
        editingQuestion = problem.Question;
        editingAnswer = problem.Answer;
        await JsRuntime.InvokeVoidAsync("edit_card_modal.showModal");
    }

    private void HandleEditSave()
    {
        if (editingIndex < 0 || string.IsNullOrWhiteSpace(editingQuestion) ||
            string.IsNullOrWhiteSpace(editingAnswer)) return;
        flashCardProblems[editingIndex] = new FlashCardProblem
        {
            Question = editingQuestion,
            Answer = editingAnswer
        };
        editingIndex = -1;
        editingQuestion = string.Empty;
        editingAnswer = string.Empty;
    }

    private void HandleEditDiscard()
    {
        editingIndex = -1;
        editingQuestion = string.Empty;
        editingAnswer = string.Empty;
    }

    private bool IsFormIncomplete()
    {
        return string.IsNullOrWhiteSpace(DeckName) ||
               string.IsNullOrWhiteSpace(SelectedCategory) ||
               string.IsNullOrWhiteSpace(DeckDescription) ||
               flashCardProblems.Count == 0;
    }


    private async Task HandleFinalize()
    {
        if (string.IsNullOrWhiteSpace(DeckName) || string.IsNullOrWhiteSpace(SelectedCategory) ||
            string.IsNullOrWhiteSpace(DeckDescription))
        {
            Console.WriteLine("Please fill all the required fields");
            return;
        }

        if (flashCardProblems.Count == 0)
        {
            Console.WriteLine("Please add at least one flashcard problem");
            return;
        }

        isSubmitting = true;

        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var googleId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(googleId))
            {
                Console.WriteLine("GoogleId claim is missing or empty.");
                return;
            }

            var newDeck = new DeckBase
            {
                Title = DeckName,
                Description = DeckDescription,
                Category = SelectedCategory,
                TotalQuestion = flashCardProblems.Count,
                IsPublic = deckVisibility,
                GoogleId = googleId
            };

            await FlashCardService.CreateNewFlashCard(newDeck, flashCardProblems);
            Navigation.NavigateTo("/");
        }
        catch (Exception ex)
        {
            // Handle error
            Console.WriteLine(ex);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private void NavigateToHome()
    {
        Navigation.NavigateTo("/", true);
    }
}