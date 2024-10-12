using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class FaqCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<RecordEmotionCommand> _logger;

    private string _text =
        $"یکی از گزینه‌های زیر را انتخاب کنید\n\n";

    public FaqCommand(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.GetFaqInlineKeyboardMarkup(),
            cancellationToken: cancellationToken);
    }
}
