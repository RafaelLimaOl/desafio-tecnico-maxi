using Desafio_WebAPI.Models.Entities;

namespace Desafio_WebAPI.Models.Response;

public class TransactionResponse
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public bool? IsActive { get; set; }

    public Guid PeopleId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    public TransactionType TransactionType { get; set; }
    public TransactionStatus Status { get; set; }
}
