using System.Security.Cryptography.X509Certificates;
using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotAction;

public class ToggleReminderCommandAction : IBotAction
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HowDoYouFeelCommandAction> _logger;
    private readonly IUserRepository _userRepository;
    private string _text = string.Empty;

    public ToggleReminderCommandAction(ITelegramBotClient botClient, ILogger<HowDoYouFeelCommandAction> logger,
        IUserRepository userRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task Execute(BotActionMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.ToggleReminder);

        var user = await _userRepository.GetByAsync(message.Username);
        _text = $"آیا مایل به {(user.IsGlobalSatisfactionReminderActive ? "غیرفعال‌سازی" : "فعال‌سازی")} یادآور هستید؟";

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.ReminderToggleInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
