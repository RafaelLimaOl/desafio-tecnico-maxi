using Desafio_WebAPI.Interfaces.Services;
using Desafio_WebAPI.Services;

namespace Desafio_WebAPI.Utils;

public static class ServiceDependecyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Adição das injeções de dependências de cada Serviço da aplicação
        services.AddScoped<IPeopleService, PeopleService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserSettingsService, UserSettingsService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}
