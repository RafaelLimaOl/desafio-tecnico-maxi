using Desafio_WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Desafio_WebAPI.Data;

public class DataContext(DbContextOptions<DataContext> options ) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<People> Peoples { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    // Adição da lógica para adicionar valores por default nas propriedades Id e CreatedAt
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Utilização do looping para não precisar repetir o mesmo código 4 vezes para cada classe

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Armazena a entidade que seja igual a: Id
            var idProperty = entity.FindProperty("Id");

            // Adiciona a propriedade NEWSEQUENTIALID() que irá fornecer um GUID
            if (idProperty != null && idProperty.ClrType == typeof(Guid))
            {
                idProperty.SetDefaultValueSql("NEWSEQUENTIALID()");
                idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
            }
        }

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var createdAt = entity.FindProperty("CreatedAt");

            if (createdAt != null && createdAt.ClrType == typeof(DateTime))
            {
                createdAt.SetDefaultValueSql("GETDATE()");
            }
        }

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var isActive = entity.FindProperty("IsActive");

            if (isActive != null && isActive.ClrType == typeof(bool?))
            {
                isActive.SetDefaultValue(true);
            }
        }

        // Organização das relações entre tabelas:

        // User → People
        modelBuilder.Entity<People>()
            .HasOne(p => p.User)
            .WithMany(u => u.Peoples)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User → Category
        modelBuilder.Entity<Category>()
            .HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // People → Transaction
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.People)
            .WithMany(p => p.Transactions)
            .HasForeignKey(t => t.PeopleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Category → Transaction (Sem Cadascade por conta da restrição do SQLServer)
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
