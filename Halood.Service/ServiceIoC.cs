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
        services.AddTransient<IBotCommand, RecordSatisfactionCommand>();
        services.AddTransient<IBotCommand, RecordEmotionCommand>();
        services.AddTransient<IBotCommand, NoCommand>();
        services.AddTransient<IBotCommand, ToggleSatisfactionReminderCommand>();
        services.AddTransient<IBotCommand, ChangeSettingsCommand>();
        services.AddTransient<IBotCommand, ChangeLanguageCommand>();
        services.AddTransient<IBotCommand, GenerateReportCommand>();
        services.AddTransient<IBotCommand, ChangeEmotionReminderCommand>();


        services.AddTransient<IBotReply, UnknownReply>();
        services.AddTransient<IBotReply, RecordSatisfactionReply>();
        services.AddTransient<IBotReply, RecordEmotionReply>();
        services.AddTransient<IBotReply, ToggleReminderReply>();
        services.AddTransient<IBotReply, ChangeLanguageReply>();
        services.AddTransient<IBotReply, ChangeEmotionReminderReply>();

        return services;
    }
}
