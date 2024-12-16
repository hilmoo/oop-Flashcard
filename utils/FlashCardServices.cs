using Supabase.Postgrest;
using flashcard.model;
using flashcard.model.Entities;

namespace flashcard.utils
{
	public class FlashCardService
	{
		private readonly Supabase.Client _supabaseClient;
		public string DeckName { get; set; } = string.Empty;
		public string SelectedCategory { get; set; } = string.Empty;

		public FlashCardService(Supabase.Client supabaseClient)
		{
			_supabaseClient = supabaseClient;
		}

		public List<FlashCardProblem> FlashCardProblems { get; private set; } = new List<FlashCardProblem>();

		public void AddFlashCardProblem(string question, string answer)
		{
			FlashCardProblems.Add(new FlashCardProblem { Question = question, Answer = answer });
		}

		public void DeleteFlashCardProblem(int index)
		{
			FlashCardProblems.RemoveAt(index);
		}

		public async Task SaveDeckToSupabase(string title, string category, int accountId)
		{
			// Create slug from title
			string slug = title.ToLower().Replace(" ", "-");
			string uuid = Guid.NewGuid().ToString();
			string slugWithUuid = $"{slug}-{uuid}";

			// Create flashcard deck
			var flashcard = new Flashcard
			{
				Slug = slugWithUuid,
				Title = title,
				Category = category,
				TotalQuestion = FlashCardProblems.Count,
				AccountId = accountId
			};

			var savedFlashcard = await _supabaseClient.From<Flashcard>()
				.Insert(flashcard, new QueryOptions { Returning = QueryOptions.ReturnType.Representation });

			// Add all problems
			foreach (var problem in FlashCardProblems)
			{
				var newProblem = new Problem
				{
					Question = problem.Question,
					Answer = problem.Answer,
					FlashcardId = savedFlashcard.Model.Id
				};

				await _supabaseClient.From<Problem>()
					.Insert(newProblem);
			}

			FlashCardProblems.Clear();
		}

		public async Task<List<Flashcard>> GetAllFlashCards()
		{
			try
			{
				var response = await _supabaseClient
					.From<Flashcard>()
					.Select("*")
					.Get();

				return response.Models;
            }
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching flashcards: {ex.Message}");
				return new List<Flashcard>();
			}
		}

		public async Task<Flashcard> GetFlashcardBySlug(string slug)
		{
			var deckName = await _supabaseClient
				.From<Flashcard>()
				.Where(x => x.Slug == slug)
				.Single();

			return deckName!;
        }
		public async Task<List<Problem>> GetProblemsByFlashcardSlug(string flashcardSlug)
		{
			try
			{
				var flashcardId = (await _supabaseClient
					.From<Flashcard>()
					.Where(x => x.Slug == flashcardSlug)
					.Get()).Models.FirstOrDefault()?.Id;
				var response = await _supabaseClient
					.From<Problem>()
					.Where(x => x.FlashcardId == flashcardId)
					.Get();

				Console.WriteLine("Jumlah problem: " + response.Models.Count);
				return response.Models;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching problems: {ex.Message}");
				return new List<Problem>();
			}
		}
	}
}