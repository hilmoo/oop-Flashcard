# SimpleDotEnv

## Usage

1. **Create a `.env` File**

   Create a `.env` file in the root of your project or any desired directory. The file should contain key-value pairs in
   the following format:

   ```plaintext
   # This is a comment
   DATABASE_URL=http://example.com/database
   API_KEY=1234567890abcdef
   DEBUG=true
    ```

2. **Load Environment Variables**

   Use the DotEnv.Load method to load the environment variables from the .env file:

    ```csharp
   using SimpleDotEnv;

   class Program
   {
   static void Main(string[] args)
   {
   // Specify the path to your .env file
   string filePath = ".env";
   
           // Load environment variables
           DotEnv.Load(filePath);
   
           // Access environment variables
           string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
           string? apiKey = Environment.GetEnvironmentVariable("API_KEY");
           string? debugMode = Environment.GetEnvironmentVariable("DEBUG");
   
           Console.WriteLine($"DATABASE_URL: {databaseUrl}");
           Console.WriteLine($"API_KEY: {apiKey}");
           Console.WriteLine($"DEBUG: {debugMode}");
       }
   
   }

   ```
   
## License

This package is free to use under the MIT License. Feel free to modify and adapt it to your needs.