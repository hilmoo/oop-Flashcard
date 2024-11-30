using flashcard.Data;
using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;

namespace flashcard.utils
{
	public class DbAccountServices(IDbContextFactory<DataContext> dbContextFactory)
	{
		private readonly IDbContextFactory<DataContext> _dbContextFactory = dbContextFactory;

		public void AddNewAccount(string email)
		{
			using var context = _dbContextFactory.CreateDbContext();
			var existingAccount = context.Set<Account>().FirstOrDefault(a => a.Email == email);
			if (existingAccount == null)
			{
				var account = new Account { Email = email };
				context.Set<Account>().Add(account);
				context.SaveChanges();
			}
		}
	}
}
