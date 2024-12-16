using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace flashcard.model.Entities
{
	[Table("flashcards")]
	class Flashcard : BaseModel
	{
		[PrimaryKey("id", false)]
		public int Id { get; set; }

		[Column("slug")]
		public string Slug { get; set; }

		[Column("title")]
		public string Title { get; set; }

		[Column("category")]
		public string Category { get; set; }

		[Column("total_question")]
		public int TotalQuestion { get; set; }

		[Column("account_id")]
		public int AccountId { get; set; }
	}
}