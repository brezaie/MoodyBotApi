using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class RecordEmotionCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<RecordEmotionCommand> _logger;

    private string _text =
        $"کدام‌یک از احساس‌های زیر به احساسی که در این لحظه تجربه می‌کنید، نزدیک‌تر است؟\n\n";

    public RecordEmotionCommand(ITelegramBotClient botClient, ILogger<RecordEmotionCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.GetBasicEmotionsInlineKeyboardMarkup(),
            cancellationToken: cancellationToken);
    }
}
