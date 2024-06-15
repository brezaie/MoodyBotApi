using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Halood.Service.BotCommand;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotReply;

public class UnknownReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = "دستوری برای اجرا انتخاب نشده است. لطفاً یکی از گزینه‌های زیر را انتخاب کنید.";

    public UnknownReply(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task Execute(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.MenuInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
