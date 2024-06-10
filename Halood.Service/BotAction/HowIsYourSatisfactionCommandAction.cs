using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotAction;

public class HowIsYourSatisfactionCommandAction : IBotAction
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowIsYourSatisfactionCommandAction> _logger;
    private string _text =
        $"کدام یک از ایموجی‌های زیر، میزان رضایت شما از لحظه‌ای که در آن هستید را می‌تواند به بهترین شکل نشان دهد؟";

    public HowIsYourSatisfactionCommandAction(ITelegramBotClient botClient,
        ILogger<HowIsYourSatisfactionCommandAction> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task Execute(BotActionMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.Satisfaction);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.SatisfactionLevelReplyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
