using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio_WebAPI.Models.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    [Required, MaxLength(100)]
    public string Email { get; set; } = null!;
    [Required, MaxLength(100)]
    public string Username { get; set; } = null!;
    [Required, MaxLength(200)]
    public string PasswordHash { get; set; } = null!;

    [MaxLength(500)]
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<People> Peoples { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
}