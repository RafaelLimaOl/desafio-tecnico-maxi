using Desafio_WebAPI.Interfaces.Repositories;
using Desafio_WebAPI.Respositories;

namespace Desafio_WebAPI.Utils;

public static class RepositoryDependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPeopleRepository, PeopleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }
}
