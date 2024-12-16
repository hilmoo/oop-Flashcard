using Supabase;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace flashcard.model.Entities
{
    [Table("problems")]
    public class Problem : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("question")]
        public string Question { get; set; }

        [Column("answer")]
        public string Answer { get; set; }

        [Column("flashcard_id")]
        public int FlashcardId { get; set; }
    }
}