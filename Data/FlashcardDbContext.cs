using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Data
{
	public class FlashcardDbContext : DbContext
	{
		public FlashcardDbContext(DbContextOptions<FlashcardDbContext> options) : base(options)
		{
		}
		public DbSet<Account> Accounts { get; set; }
	}
}
