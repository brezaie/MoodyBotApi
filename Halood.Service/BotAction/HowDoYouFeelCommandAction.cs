using Halood.Domain.Dtos;
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
        $"این قسمت بزودی تکمیل می‌شود";

    public HowDoYouFeelCommandAction(ITelegramBotClient botClient, ILogger<HowDoYouFeelCommandAction> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task Execute(BotActionMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
    }
}
