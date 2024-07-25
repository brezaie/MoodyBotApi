using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class ToggleReminderCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowDoYouFeelCommand> _logger;
    private readonly IUserRepository _userRepository;
    private string _text = string.Empty;

    public ToggleReminderCommand(ITelegramBotClient botClient, ILogger<HowDoYouFeelCommand> logger,
        IUserRepository userRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.Reminder);

        var user = await _userRepository.GetByAsync(message.Username);
        _text = $"آیا مایل به {(user.IsGlobalSatisfactionReminderActive ? "غیرفعال‌سازی" : "فعال‌سازی")} یادآور هستید؟";

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.ReminderToggleInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
