using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class HowIsYourSatisfactionCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowIsYourSatisfactionCommand> _logger;
    private string _text =
        $"کدام یک از ایموجی‌های زیر، میزان رضایت شما از امروز‌تان را می‌تواند به بهترین شکل نشان دهد؟";

    public HowIsYourSatisfactionCommand(ITelegramBotClient botClient,
        ILogger<HowIsYourSatisfactionCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task Execute(BotCommandMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.Satisfaction);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.SatisfactionLevelReplyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
