using flashcard.Data;
using flashcard.model.Entities;
using Microsoft.EntityFrameworkCore;
using Supabase.Postgrest;

namespace flashcard.utils
{
	public class DbAccountServices
	{
		private readonly IDbContextFactory<DataContext> _dbContextFactory;
		private readonly Supabase.Client _supabaseClient;

		public DbAccountServices(IDbContextFactory<DataContext> dbContextFactory, Supabase.Client supabaseClient)
		{
			_dbContextFactory = dbContextFactory;
			_supabaseClient = supabaseClient;
		}

		public void AddNewAccount(string email)
		{
			using var context = _dbContextFactory.CreateDbContext();

			// Check if the account exists in the local database
			var existingAccount = context.Set<Account>().FirstOrDefault(a => a.Email == email);

			if (existingAccount == null)
			{
				// Add new account to local database
				var account = new Account { Email = email };
				context.Set<Account>().Add(account);

				// Save local changes
				context.SaveChanges();
			}
		}

		// Mark this method as async
		public async Task InsertAccountToSupabase(string email, string googleId, string name)
		{
			var accountData = new SupabaseAccount
			{
				Email = email,
				GoogleId = googleId,
				Name = name
			};

			var count = await _supabaseClient
					.From<SupabaseAccount>()
					.Select(x => new object[] { x.Email })
					.Where(x => x.Email == email)
					.Count(Constants.CountType.Exact);

			if (count == 0)
			{
				await _supabaseClient.From<SupabaseAccount>().Insert(accountData);
			}
		}
	}
}
