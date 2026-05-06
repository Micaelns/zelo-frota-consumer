using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class RegisterContext
{
    public static IServiceCollection AddContexts(this IServiceCollection services, string? sqlQueryString)
    {
        services.AddDbContext<ZeloFrotaDbContext>((sp, options) =>
        {
            options.UseSqlServer(sqlQueryString);
    }
        );
        return services;
    }
}
