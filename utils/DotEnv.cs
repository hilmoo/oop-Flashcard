namespace flashcard.utils
{
	using System;
	using System.IO;

	public static class DotEnv
	{
		public static void Load(string filePath)
		{
			if (!File.Exists(filePath))
				return;

			foreach (var line in File.ReadAllLines(filePath))
			{
				// Ignore empty lines and comments
				if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
					continue;

				// Find the first '=' to split the key and value
				var indexOfEquals = line.IndexOf('=');
				if (indexOfEquals <= 0 || indexOfEquals == line.Length - 1)
					continue; // Skip lines without a valid key=value format

				var key = line.Substring(0, indexOfEquals).Trim();
				var value = line.Substring(indexOfEquals + 1).Trim().Trim('"');

				if (!string.IsNullOrEmpty(key) && value != null)
				{
					Environment.SetEnvironmentVariable(key, value);
				}
			}
		}
	}
}