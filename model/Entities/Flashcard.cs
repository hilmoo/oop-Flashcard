using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace flashcard.model.Entities
{
    [Table("flashcards")]
    public class Flashcard : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("slug")]
        public string Slug { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("total_question")]
        public int TotalQuestion { get; set; }

        [Column("account_id")]
        public int AccountId { get; set; }
    }
}