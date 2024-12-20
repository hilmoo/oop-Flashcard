using System.Text.RegularExpressions;
using flashcard.model;
using flashcard.model.Entities;
using flashcard.utils;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Data;

public partial class FlashCardService(IDbContextFactory<DataContext> dbContextFactory)
{
    private readonly IDbContextFactory<DataContext> dbContextFactory =
        dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

    public async Task CreateNewFlashCard(DeckBase flashcardData, List<FlashCardProblem> flashCardProblems)
    {
        ArgumentNullException.ThrowIfNull(flashcardData);

        if (flashCardProblems == null || flashCardProblems.Count == 0)
            throw new ArgumentException("FlashCardProblems cannot be null or empty.", nameof(flashCardProblems));

        await using var context = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var slug = SafeCharRegex().Replace(flashcardData.Title.ToLower(), "").Replace(" ", "-") + "-" +
                       CustomRandom.GenerateRandomString(6);

            var accountId = context.Set<Account>().FirstOrDefault(a => a.GoogleId == flashcardData.GoogleId)?.Id;
            if (accountId == null)
                throw new Exception("Account not found");

            var newDeck = new Deck
            {
                Title = flashcardData.Title,
                Description = flashcardData.Description,
                Category = flashcardData.Category,
                TotalQuestion = flashcardData.TotalQuestion,
                IsPublic = flashcardData.IsPublic,
                AccountId = accountId.Value,
                Slug = slug
            };

            var savedDeck = context.Set<Deck>().Add(newDeck);
            await context.SaveChangesAsync();

            var problems = flashCardProblems.Select(problem => new FlashCard
            {
                Question = problem.Question,
                Answer = problem.Answer,
                DeckId = savedDeck.Entity.Id
            }).ToList();

            context.Set<FlashCard>().AddRange(problems);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateFlashCard(string slug, DeckBase flashcardData, List<FlashCard> flashCardProblems)
    {
        ArgumentNullException.ThrowIfNull(flashcardData);

        if (flashCardProblems == null || flashCardProblems.Count == 0)
            throw new ArgumentException("FlashCardProblems cannot be null or empty.", nameof(flashCardProblems));

        await using var context = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var accountId = context.Set<Account>().FirstOrDefault(a => a.GoogleId == flashcardData.GoogleId)?.Id;
            if (accountId == null)
                throw new Exception("Account not found");

            var deck = await context.Set<Deck>()
                .FirstOrDefaultAsync(d => d.Slug == slug);

            var flashCards = await context.Set<FlashCard>()
                .Where(fc => fc.DeckId == deck!.Id)
                .ToListAsync();

            if (deck == null)
                throw new Exception("Deck not found");

            // Update deck properties
            deck.Slug = SafeCharRegex().Replace(flashcardData.Title.ToLower(), "").Replace(" ", "-") + "-" +
                        CustomRandom.GenerateRandomString(6);
            deck.Title = flashcardData.Title;
            deck.Description = flashcardData.Description;
            deck.Category = flashcardData.Category;
            deck.TotalQuestion = flashcardData.TotalQuestion;
            deck.IsPublic = flashcardData.IsPublic;
            deck.AccountId = accountId.Value;

            // Remove all existing flashcards for this deck
            if (flashCards.Count != 0)
            {
                context.Set<FlashCard>().RemoveRange(flashCards);
            }

            // Create new flashcards
            var problems = flashCardProblems.Select(problem => new FlashCard
            {
                Question = problem.Question,
                Answer = problem.Answer,
                DeckId = deck.Id
            }).ToList();

            // Add new flashcards
            context.Set<FlashCard>().AddRange(problems);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteDeck(string email, string slug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        var currentAccountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        var deckId = await context.Set<Deck>()
            .Where(d => d.Slug == slug)
            .Select(d => d.AccountId)
            .FirstOrDefaultAsync();

        if (currentAccountId != deckId)
            throw new Exception("You are not authorized to delete this deck");

        try
        {
            var deck = await context.Set<Deck>()
                .FirstOrDefaultAsync(d => d.Slug == slug);

            if (deck == null)
                throw new Exception("Deck not found");

            var deckMarks = await context.Set<DeckMark>()
                .Where(dm => dm.DeckId == deck.Id)
                .ToListAsync();

            if (deckMarks.Count != 0)
            {
                context.Set<DeckMark>().RemoveRange(deckMarks);
                await context.SaveChangesAsync();
            }

            var flashCards = await context.Set<FlashCard>()
                .Where(fc => fc.DeckId == deck.Id)
                .ToListAsync();

            if (flashCards.Count != 0)
            {
                context.Set<FlashCard>().RemoveRange(flashCards);
                await context.SaveChangesAsync();
            }

            context.Set<Deck>().Remove(deck);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            Console.WriteLine("Error deleting deck");
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Deck>> GetAllDecks(string? email)
    {
        if (email == null)
        {
            await using var pubContext = await dbContextFactory.CreateDbContextAsync();
            var pubDecks = await pubContext.Set<Deck>().ToListAsync();
            return pubDecks.Where(d => d.IsPublic).ToList();
        }

        await using var context = await dbContextFactory.CreateDbContextAsync();
        var currentAccountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (currentAccountId == 0)
            throw new Exception("Current account not found");

        var decks = await context.Set<Deck>().ToListAsync();

        return decks.Where(d => d.IsPublic || d.AccountId == currentAccountId).ToList();
    }

    public async Task<string> GetAuthorByAccountId(int accountId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
#pragma warning disable CS8603 // Possible null reference return.
        return await context.Set<Account>()
            .Where(a => a.Id == accountId)
            .Select(a => a.Name)
            .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
    }

    public async Task<List<Deck>> GetAllDecksByEmail(string email)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var accountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (accountId == 0)
            throw new Exception("Account not found");

        return await context.Set<Deck>()
            .Where(d => d.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<bool> IsCanSeeDeck(string email, string deckSlug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var accountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        var deckData = await context.Set<Deck>()
            .Where(d => d.Slug == deckSlug)
            .FirstOrDefaultAsync();

        if (deckData == null)
        {
            return false;
        }

        if (deckData.IsPublic)
        {
            return true;
        }

        return deckData.AccountId == accountId;
    }

    public async Task<bool> IsCanEditDeck(string email, string deckSlug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var accountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (accountId == 0)
            throw new Exception("Account not found");

        var deckData = await context.Set<Deck>()
            .Where(d => d.Slug == deckSlug)
            .FirstOrDefaultAsync();

        if (deckData == null)
        {
            return false;
        }

        return deckData.AccountId == accountId;
    }

    public async Task<Deck> GetDeckBySlug(string slug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Set<Deck>().FirstOrDefaultAsync(d => d.Slug == slug) ??
               throw new Exception("Flashcard not found");
    }

    public async Task<List<FlashCard>> GetFlashcardByDeckSlug(string flashcardSlug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var flashcardId = await context.Set<Deck>()
            .Where(d => d.Slug == flashcardSlug)
            .Select(d => d.Id)
            .FirstOrDefaultAsync();

        if (flashcardId == 0)
            throw new Exception("Flashcard not found");

        return await context.Set<FlashCard>()
            .Where(fc => fc.DeckId == flashcardId)
            .ToListAsync();
    }

    public async Task<List<FlashCard>> GetFlashcardByDeckId(int deckId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Set<FlashCard>()
            .Where(fc => fc.DeckId == deckId)
            .ToListAsync();
    }

    public async Task SetDeckMark(string email, string deckSlug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var accountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (accountId == 0)
            throw new Exception("Account not found");

        var deckId = await context.Set<Deck>()
            .Where(d => d.Slug == deckSlug)
            .Select(d => d.Id)
            .FirstOrDefaultAsync();

        if (deckId == 0)
            throw new Exception("Deck not found");

        var deckMark = new DeckMark
        {
            DeckId = deckId,
            AccountId = accountId
        };

        context.Set<DeckMark>().Add(deckMark);
        await context.SaveChangesAsync();
    }

    public async Task RemoveDeckMark(string email, string deckSlug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var accountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (accountId == 0)
            throw new Exception("Account not found");

        var deckId = await context.Set<Deck>()
            .Where(d => d.Slug == deckSlug)
            .Select(d => d.Id)
            .FirstOrDefaultAsync();

        if (deckId == 0)
            throw new Exception("Deck not found");

        var deckMark = await context.Set<DeckMark>()
            .FirstOrDefaultAsync(dm => dm.DeckId == deckId && dm.AccountId == accountId);

        if (deckMark == null)
            throw new Exception("Deck not marked");

        context.Set<DeckMark>().Remove(deckMark);
        await context.SaveChangesAsync();
    }

    public async Task<bool> IsDeckMarked(string email, string deckSlug)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var accountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (accountId == 0)
            throw new Exception("Account not found");

        var deckId = await context.Set<Deck>()
            .Where(d => d.Slug == deckSlug)
            .Select(d => d.Id)
            .FirstOrDefaultAsync();

        if (deckId == 0)
            throw new Exception("Deck not found");

        return await context.Set<DeckMark>()
            .AnyAsync(dm => dm.DeckId == deckId && dm.AccountId == accountId);
    }

    public async Task<List<Deck>> GetMarkedDecks(string email)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var accountId = await context.Set<Account>()
            .Where(a => a.Email == email)
            .Select(a => a.Id)
            .FirstOrDefaultAsync();

        if (accountId == 0)
            throw new Exception("Account not found");

        var deckIds = await context.Set<DeckMark>()
            .Where(dm => dm.AccountId == accountId)
            .Select(dm => dm.DeckId)
            .ToListAsync();

        return await context.Set<Deck>()
            .Where(d => deckIds.Contains(d.Id))
            .ToListAsync();
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex SafeCharRegex();
}