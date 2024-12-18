using System.Text.RegularExpressions;
using flashcard.model;
using flashcard.model.Entities;
using flashcard.utils;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Data
{
    public partial class FlashCardService(IDbContextFactory<DataContext> dbContextFactory)
    {
        private readonly IDbContextFactory<DataContext> dbContextFactory =
            dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

        public async Task CreateNewFlashCard(FlashCardBase flashcardData, List<FlashCardProblem> flashCardProblems)
        {
            ArgumentNullException.ThrowIfNull(flashcardData);

            if (flashCardProblems == null || flashCardProblems.Count == 0)
                throw new ArgumentException("FlashCardProblems cannot be null or empty.", nameof(flashCardProblems));

            await using var context = await dbContextFactory.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var slug = SafeCharRegex().Replace(flashcardData.Title.ToLower(), "")
                               .Replace(" ", "-")
                           + "-" + CustomRandom.GenerateRandomString(6);

                var accountId = context.Set<Account>().FirstOrDefault(a => a.GoogleId == flashcardData.GoogleId)?.Id;
                if (accountId == null)
                    throw new Exception("Account not found");

                var newFlashCard = new FlashCard
                {
                    Title = flashcardData.Title,
                    Description = flashcardData.Description,
                    Category = flashcardData.Category,
                    TotalQuestion = flashcardData.TotalQuestion,
                    IsPublic = flashcardData.IsPublic,
                    AccountId = accountId.Value,
                    Slug = slug
                };

                var savedFlashcard = context.Set<FlashCard>().Add(newFlashCard);

                var problems = flashCardProblems.Select(problem => new FlashCardProblems
                {
                    Question = problem.Question,
                    Answer = problem.Answer,
                    FlashcardId = savedFlashcard.Entity.Id
                }).ToList();

                context.Set<FlashCardProblems>().AddRange(problems);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Flashcard>> GetAllFlashCards()
        {
            try
            {
                var response = await _supabaseClient
                    .From<Flashcard>()
                    .Select("*")
                    .Get();

                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching flashcards: {ex.Message}");
                return new List<Flashcard>();
            }
        }

        public async Task<Flashcard> GetFlashcardBySlug(string slug)
        {
            var deckName = await _supabaseClient
                .From<Flashcard>()
                .Where(x => x.Slug == slug)
                .Single();

            return deckName!;
        }

        public async Task<List<Problem>> GetProblemsByFlashcardSlug(string flashcardSlug)
        {
            try
            {
                var flashcardId = (await _supabaseClient
                    .From<Flashcard>()
                    .Where(x => x.Slug == flashcardSlug)
                    .Get()).Models.FirstOrDefault()?.Id;
                var response = await _supabaseClient
                    .From<Problem>()
                    .Where(x => x.FlashcardId == flashcardId)
                    .Get();

                Console.WriteLine("Jumlah problem: " + response.Models.Count);
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching problems: {ex.Message}");
                return new List<Problem>();
            }
        }

        [GeneratedRegex(@"[^a-z0-9\s-]")]
        private static partial Regex SafeCharRegex();
    }
}