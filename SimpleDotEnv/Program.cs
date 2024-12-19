namespace SimpleDotEnv;

using System;
using System.IO;

/// <summary>
/// Provides functionality to load environment variables from a `.env` file into the application's environment.
/// </summary>
public static class DotEnv
{
    /// <summary>
    /// Loads environment variables from a specified file and sets them
    /// into the process's environment variables.
    /// </summary>
    /// <param name="filePath">
    /// The path to the file containing environment variables in
    /// key-value pairs separated by an equals sign (`KEY=VALUE`).
    /// </param>
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (IsCommentOrEmptyLine(line))
                continue;

            var (key, value) = ParseKeyValue(line);
            if (!string.IsNullOrEmpty(key))
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }

    /// <summary>
    /// Determines whether the provided line is a comment or an empty line.
    /// </summary>
    /// <param name="line">The line of text to evaluate.</param>
    /// <returns>
    /// True if the line is either a comment (starting with a '#') or consists only of whitespace; otherwise, false.
    /// </returns>
    private static bool IsCommentOrEmptyLine(string line) =>
        string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith($"#");

    /// Parses a line into a key-value pair where the key and value are separated by an '=' character.
    /// <param name="line">The input line to parse. It is expected to contain a key-value pair in the format "KEY=VALUE".</param>
    /// <returns>A tuple containing the key and value extracted from the line. If the line is not in a valid format, both key and value will be returned as empty strings.</returns>
    private static (string Key, string Value) ParseKeyValue(string line)
    {
        var indexOfEquals = line.IndexOf('=');
        if (indexOfEquals <= 0 || indexOfEquals == line.Length - 1)
            return (string.Empty, string.Empty);

        var key = line[..indexOfEquals].Trim();
        var value = line[(indexOfEquals + 1)..].Trim();

        return (key, value);
    }
}