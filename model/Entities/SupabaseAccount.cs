using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace flashcard.model.Entities
{
    [Table("accounts")]
    class SupabaseAccount : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("google_openid")]
        public string GoogleId { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}