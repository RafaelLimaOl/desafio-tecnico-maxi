using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_WebAPI.Models.Entities;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [Required, MaxLength(100)]
    public string Description { get; set; } = null!;
    public CategoryType CategoryType { get; set; }
    public bool? IsActive { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<Transaction> Transactions { get; set; } = [];
}

public enum CategoryType
{
    DESPESA = 0,
    RECEITA = 1,
    AMBAS = 2
}
