using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class HowDoYouFeelCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowDoYouFeelCommand> _logger;
    private string _text =
        $"کدام‌یک از احساس‌های زیر به احساسی که در این لحظه تجربه می‌کنید، نزدیک‌تر است؟\n\n" +
        $"برای دیدن لیست کامل احساس‌ها، اسکرول کنید.\n";

    public HowDoYouFeelCommand(ITelegramBotClient botClient, ILogger<HowDoYouFeelCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.Emotion);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.EmotionInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
