using System.Security.Claims;
using flashcard.model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using flashcard.utils;
using flashcard.model.Entities;

namespace flashcard.Components.Pages
{
    public partial class Create : ComponentBase
    {
        private string? DeckName { get; set; }
        private string? DeckDescription { get; set; }
        private string? SelectedCategory { get; set; }
        private bool deckVisibility = true;
        private bool isSubmitting = false;
        private string tempQuestion = string.Empty;
        private string tempAnswer = string.Empty;
        private List<FlashCardProblem> flashCardProblems = [];

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (!authState.User.Identity!.IsAuthenticated)
            {
                Navigation.NavigateTo("/auth/signin");
            }
        }

        private void HandleQuestionChange(ChangeEventArgs e)
        {
            tempQuestion = e.Value?.ToString() ?? string.Empty;
        }

        private void HandleAnswerChange(ChangeEventArgs e)
        {
            tempAnswer = e.Value?.ToString() ?? string.Empty;
        }

        private void HandleDiscard()
        {
            tempQuestion = string.Empty;
            tempAnswer = string.Empty;
        }

        private void HandleSave()
        {
            if (!string.IsNullOrWhiteSpace(tempQuestion) && !string.IsNullOrWhiteSpace(tempAnswer))
            {
                AddFlashCardProblem(tempQuestion, tempAnswer);
                tempQuestion = string.Empty;
                tempAnswer = string.Empty;
            }
        }

        private void AddFlashCardProblem(string question, string answer)
        {
            flashCardProblems.Add(new FlashCardProblem { Question = question, Answer = answer });
        }

        private void DeleteFlashCardProblem(int index)
        {
            flashCardProblems.RemoveAt(index);
        }

        private async Task HandleFinalize()
        {
            if (string.IsNullOrWhiteSpace(DeckName) || string.IsNullOrWhiteSpace(SelectedCategory) ||
                string.IsNullOrWhiteSpace(DeckDescription))
            {
                // Show error message
                return;
            }

            if (flashCardProblems.Count == 0)
            {
                // Show error message
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
                    // Show error message
                    return;
                }

                var newDeck = new FlashCardBase
                {
                    Title = DeckName,
                    Description = DeckDescription,
                    Category = SelectedCategory,
                    TotalQuestion = flashCardProblems.Count,
                    IsPublic = deckVisibility,
                    GoogleId = googleId,
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
    }
}