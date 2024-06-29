using Halood.Domain.Interfaces.BotAction;
using Halood.Service.BotCommand;
using Halood.Service.BotReply;
using Microsoft.Extensions.DependencyInjection;

namespace Halood.Service;

public static class ServiceIoC
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        services.AddTransient<IBotCommand, StartCommand>();
        services.AddTransient<IBotCommand, HowIsYourSatisfactionCommand>();
        services.AddTransient<IBotCommand, HowDoYouFeelCommand>();
        services.AddTransient<IBotCommand, NoCommand>();
        services.AddTransient<IBotCommand, ToggleReminderCommand>();
        services.AddTransient<IBotCommand, ChangeSettingsCommand>();
        services.AddTransient<IBotCommand, ChangeLanguageCommand>();
        services.AddTransient<IBotCommand, GenerateReportCommand>();


        services.AddTransient<IBotReply, UnknownReply>();
        services.AddTransient<IBotReply, HowIsYourSatisfactionReply>();
        services.AddTransient<IBotReply, HowDoYouFeelReply>();
        services.AddTransient<IBotReply, ToggleReminderReply>();
        services.AddTransient<IBotReply, ChangeLanguageReply>();

        return services;
    }
}
