using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flashcard.model.Entities
{
    [Table("flashcards")]
    public record FlashCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; init; }

        [Required] [Column("slug")] public string? Slug { get; set; }

        [Required] [Column("title")] public string? Title { get; set; }

        [Required] [Column("category")] public string? Category { get; set; }

        [Required] [Column("total_question")] public int TotalQuestion { get; set; }

        [Required] [Column("account_id")] public int AccountId { get; set; }
    }
}