using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class ToggleSatisfactionReminderCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<ToggleSatisfactionReminderCommand> _logger;
    private readonly IUserRepository _userRepository;
    private string _text = string.Empty;

    public ToggleSatisfactionReminderCommand(ITelegramBotClient botClient, ILogger<ToggleSatisfactionReminderCommand> logger,
        IUserRepository userRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByAsync(message.Username);
        _text = $"آیا مایل به {(user.IsGlobalSatisfactionReminderActive ? "غیرفعال‌سازی" : "فعال‌سازی")} یادآور هستید؟";

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.SatisfactionReminderToggleInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
