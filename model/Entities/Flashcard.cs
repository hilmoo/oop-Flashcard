using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flashcard.model.Entities
{
    [Table("flashcard")]
    public record FlashCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; init; }

        [Required] [Column("slug")] public string? Slug { get; set; }
        [Required] [Column("title")] public string? Title { get; set; }
        [Required] [Column("description")] public string? Description { get; set; }
        [Required] [Column("category")] public string? Category { get; set; }
        [Required] [Column("total_question")] public int TotalQuestion { get; set; }
        [Required] [Column("is_public")] public bool IsPublic { get; set; }
        [Required] [Column("account_id")] public string? GoogleId { get; set; }
    }

    [Table("flashcards_problem")]
    public record FlashCardProblems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; init; }

        [Required] [Column("question")] public string? Question { get; set; }
        [Required] [Column("answer")] public string? Answer { get; set; }
        [Required] [Column("flashcard_id")] public int FlashcardId { get; set; }
    }
}