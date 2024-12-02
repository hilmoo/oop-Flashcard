namespace flashcard.utils
{
	public class AuthenticationConstants
	{
		public const string CookieName = "auth";
	}
	public class PostgresConstants
	{
		private static readonly string Host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? throw new InvalidOperationException("POSTGRES_HOST environment variable is not set.");
		private static readonly string Database = Environment.GetEnvironmentVariable("POSTGRES_DB_NAME") ?? throw new InvalidOperationException("POSTGRES_DB_NAME environment variable is not set.");
		private static readonly string Username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? throw new InvalidOperationException("POSTGRES_USER environment variable is not set.");
		private static readonly string Password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? throw new InvalidOperationException("POSTGRES_PASSWORD environment variable is not set.");
		public static readonly string ConnectionString = $"Host={Host};Username={Username};Password={Password};Database={Database}";
	}
}
