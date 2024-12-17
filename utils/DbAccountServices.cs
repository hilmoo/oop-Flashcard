using flashcard.Data;
using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;

namespace flashcard.utils
{
    public class DbAccountServices(IDbContextFactory<DataContext> dbContextFactory)
    {
        private readonly IDbContextFactory<DataContext> dbContextFactory = dbContextFactory;

        public void AddNewAccount(string email, string name)
        {
            using var context = dbContextFactory.CreateDbContext();

            // Check if the account exists in the local database
            var existingAccount = context.Set<Account>().FirstOrDefault(a => a.Email == email);

            if (existingAccount == null)
            {
                AddAccount(context, new Account { Email = email, Name = name });
            }
        }

        private static void AddAccount(DataContext context, Account account)
        {
            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
    }
}