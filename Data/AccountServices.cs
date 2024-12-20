using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;

namespace flashcard.Data;

public class AccountServices(IDbContextFactory<DataContext> dbContextFactory)
{
    public void AddNewAccount(Account accountData)
    {
        using var context = dbContextFactory.CreateDbContext();

        // Check if the account exists in the local database
        var existingAccount = context.Set<Account>().FirstOrDefault(a => a.Email == accountData.Email);

        if (existingAccount != null) return;
        context.Set<Account>().Add(accountData);
        context.SaveChanges();
    }
}