using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class ChangeLanguageCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<RecordSatisfactionCommand> _logger;
    private string _text =
        $"کدام یک از ایموجی‌های زیر، میزان رضایت شما از امروز‌تان را می‌تواند به بهترین شکل نشان دهد؟";

    public ChangeLanguageCommand(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.Language);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.LanguageInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
