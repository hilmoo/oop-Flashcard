namespace flashcard.utils;

public static class AuthenticationConstants
{
    public const string CookieName = "auth";
}

public static class PostgresConstants
{
    private static readonly string IsUsingServer = Environment.GetEnvironmentVariable("POSTGRES_USING_SERVER")
                                                   ?? throw new InvalidOperationException(
                                                       "POSTGRES_USING_SERVER environment variable is not set.");

    public static readonly string ConnectionString;

    static PostgresConstants()
    {
        switch (IsUsingServer)
        {
            case "true":
            {
                var database = GetEnvironmentVariableOrThrow("POSTGRES_DB_NAME");
                var password = GetEnvironmentVariableOrThrow("POSTGRES_PASSWORD");
                var userId = GetEnvironmentVariableOrThrow("POSTGRES_USER_ID");
                var server = GetEnvironmentVariableOrThrow("POSTGRES_SERVER");
                var port = GetEnvironmentVariableOrThrow("POSTGRES_PORT");

                ConnectionString =
                    $"User Id={userId};Password={password};Server={server};Port={port};Database={database};Include Error Detail=True";
                break;
            }
            case "false":
            {
                var host = GetEnvironmentVariableOrThrow("POSTGRES_HOST");
                var database = GetEnvironmentVariableOrThrow("POSTGRES_DB_NAME");
                var username = GetEnvironmentVariableOrThrow("POSTGRES_USER");
                var password = GetEnvironmentVariableOrThrow("POSTGRES_PASSWORD");

                ConnectionString =
                    $"Host={host};Username={username};Password={password};Database={database}";
                break;
            }
            default:
                throw new InvalidOperationException(
                    "POSTGRES_USING_SERVER environment variable must be 'true' or 'false'.");
        }
    }

    private static string GetEnvironmentVariableOrThrow(string variableName)
    {
        return Environment.GetEnvironmentVariable(variableName)
               ?? throw new InvalidOperationException($"{variableName} environment variable is not set.");
    }
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