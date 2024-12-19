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
		private static readonly string UserId = Environment.GetEnvironmentVariable("POSTGRES_USER_ID") ?? throw new InvalidOperationException("POSTGRES_USER_ID environment variable is not set.");
		private static readonly string Server = Environment.GetEnvironmentVariable("POSTGRES_SERVER") ?? throw new InvalidOperationException("POSTGRES_SERVER environment variable is not set.");
		private static readonly string Port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? throw new InvalidOperationException("POSTGRES_PORT environment variable is not set.");
		public static readonly string ConnectionString = $"User Id={UserId};Password={Password};Server={Server};Port={Port};Database={Database};Include Error Detail=True";
    }

    public static class CustomRandom
    {
        public static string GenerateRandomString(int length)
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}