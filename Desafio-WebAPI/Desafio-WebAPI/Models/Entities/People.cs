using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_WebAPI.Models.Entities;

public class People
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [Required, MaxLength(150)]
    public string Name { get; set; } = null!;
    [Range(0, 120)]
    public int Age { get; set; }
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public User User { get; set; } = null!;
    public Guid UserId { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = [];
}
