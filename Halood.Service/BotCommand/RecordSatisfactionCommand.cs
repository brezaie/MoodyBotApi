using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class RecordSatisfactionCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<RecordSatisfactionCommand> _logger;
    private string _text =
        $"کدام یک از گزینه‌های زیر، میزان رضایت شما از امروز‌تان را می‌تواند به بهترین شکل نشان دهد؟";

    public RecordSatisfactionCommand(ITelegramBotClient botClient,
        ILogger<RecordSatisfactionCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.SatisfactionLevelInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
