using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}