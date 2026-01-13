using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_WebAPI.Models.Entities;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [Required, MaxLength(200)]
    public string Description { get; set; } = null!;
    [Column(TypeName = "decimal(18,4)")]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; } = TransactionType.DESPESA;

    public Guid PeopleId { get; set; }
    public People People { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid UserId { get; set; }
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public TransactionStatus Status { get; set; } = TransactionStatus.PENDENTE;
}

public enum TransactionType
{
    DESPESA,
    RECEITA
}

public enum TransactionStatus
{
    PENDENTE,
    CONCLUIDA,
    ATRASADA,
    CANCELADA
}
