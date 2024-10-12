using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Halood.Service.BotCommand;

public class StartCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<StartCommand> _logger;

    private string _text =
        $"به حالود خوش آمدید!\n\nبرای مطلع شدن از نحوه کار با بات و دیگر سوال‌ها، از طریق گزینه Menu (قسمت پایین)، گزینه \"سوالات متداول\" را انتخاب کنید.\n";
    public StartCommand(ITelegramBotClient botClient, ILogger<StartCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
    
    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendChatActionAsync(
            chatId: message.ChatId,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);

        // Simulate longer running task
        await Task.Delay(1000, cancellationToken);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
