using Halood.Domain.Interfaces;
using Halood.Service.BotAction;
using Microsoft.Extensions.DependencyInjection;

namespace Halood.Service;

public static class ServiceIoC
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddTransient<IBotAction, StartCommandAction>();
        services.AddTransient<IBotAction, HowIsYourSatisfactionCommandAction>();

        return services;
    }
}
