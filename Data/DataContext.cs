using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<FlashCard> FlashCards { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<DeckMark> DeckMarks { get; set; }
}