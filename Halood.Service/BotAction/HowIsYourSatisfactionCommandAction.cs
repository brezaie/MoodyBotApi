using Halood.Common;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Halood.Service.BotAction;

public class HowIsYourSatisfactionCommandAction : IBotAction
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowIsYourSatisfactionCommandAction> _logger;
    private string _text =
        $"چه میزان از این لحظه‌ای که در آن هستی، رضایت داری؟ \n";

    public HowIsYourSatisfactionCommandAction(ITelegramBotClient botClient,
        ILogger<HowIsYourSatisfactionCommandAction> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task Execute(Message message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Chat.Username, CommandType.Satisfaction);

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: _text,
            replyMarkup: CommandHandler.SatisfactionLevelReplyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
