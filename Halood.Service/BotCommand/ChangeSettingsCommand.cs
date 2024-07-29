using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Service.BotCommand;

public class ChangeSettingsCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowDoYouFeelCommand> _logger;
    private string _text = string.Empty;

    public ChangeSettingsCommand(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        _text = $"برای تغییر هر یک از تنظیمات، روی آن کلیک کنید";

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.SettingsInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
