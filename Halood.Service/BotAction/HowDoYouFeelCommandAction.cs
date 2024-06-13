using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Service.BotAction;

public class HowDoYouFeelCommandAction : IBotAction
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowDoYouFeelCommandAction> _logger;
    private string _text =
        $"کدام‌یک از احساس‌های زیر به احساسی که در این لحظه تجربه می‌کنید، نزدیک‌تر است؟\n\n" +
        $"برای دیدن لیست کامل احساس‌ها، اسکرول کنید.\n";

    public HowDoYouFeelCommandAction(ITelegramBotClient botClient, ILogger<HowDoYouFeelCommandAction> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task Execute(BotActionMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.Emotion);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.EmotionReplyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
