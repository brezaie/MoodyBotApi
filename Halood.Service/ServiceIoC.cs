using Halood.Domain.Interfaces;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Repository;
using Halood.Repository.EntityFramework;
using Halood.Service.BotAction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Halood.Service;

public static class ServiceIoC
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddTransient<IBotAction, StartCommandAction>();

        return services;
    }
}
