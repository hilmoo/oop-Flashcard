using flashcard.Components.Pages;
using flashcard.Data;
using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace flashcard.Tests;

public class DbTest : TestContext
{
    [Fact]
    private void TestAddAccount()
    {
        Services.AddDbContextFactory<DataContext>(options => { options.UseInMemoryDatabase("FakeDatabase"); });
        Services.AddSingleton<AccountServices>();
        var accountService = Services.GetRequiredService<AccountServices>();

        var account = new Account()
        {
            Email = "test@mail.com",
            GoogleId = "123",
            Name = "Test User"
        };

        accountService.AddNewAccount(account);

        using var context = Services.GetRequiredService<IDbContextFactory<DataContext>>().CreateDbContext();
        var savedAccount = context.Accounts.FirstOrDefault(a => a.Email == account.Email);
        Assert.NotNull(savedAccount); // Ensure account was added
        Assert.Equal(account.Email, savedAccount.Email); // Verify Email
        Assert.Equal(account.GoogleId, savedAccount.GoogleId); // Verify GoogleId
        Assert.Equal(account.Name, savedAccount.Name); // Verify Name
    }
}