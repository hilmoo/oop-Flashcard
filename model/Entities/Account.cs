using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flashcard.model.Entities
{
    [Table("accounts")]
    public record Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; init; }

        [Required] [Column("email")] public string? Email { get; init; }

        [Required] [Column("name")] public string? Name { get; init; }
    }
}