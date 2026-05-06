using Application.Contracts;
using Infra.Data.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class ImplementsExtensions
{
    public static IServiceCollection ImplementsRepository(this IServiceCollection services)
    {
        services.AddScoped<ITravelQuery, TravelQuery>();

        return services;
    }
}
