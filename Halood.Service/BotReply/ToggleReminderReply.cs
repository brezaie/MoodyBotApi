using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Service.BotCommand;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotReply;

public class ToggleReminderReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = string.Empty;

    private readonly IUserRepository _userRepository;


    public ToggleReminderReply(ITelegramBotClient botClient, IUserRepository userRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
    }

    public async Task Execute(BotCommandMessage message, CancellationToken cancellationToken)
    {
        // اگر متن وارد شده، هیچ یک از گزینه های پیشنهادی نبود
        if (((YesNoResponse[])Enum.GetValues(typeof(YesNoResponse))).All(x =>
                x.GetDescription() != message.Text))
        {
            _text = $"گزینه انتخاب شده نادرست می‌باشد. لطفاً یکی از گزینه‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: CommandHandler.SatisfactionReminderToggleInlineKeyboardMarkup,
                cancellationToken: cancellationToken);
            return;
        }

        if (message.Text == YesNoResponse.No.GetDescription())
        {
            _text = $"دستور مورد نظر لغو گردید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                cancellationToken: cancellationToken);

            CommandHandler.RemoveCommand(message.Username);

            return;
        }

        var user = await _userRepository.GetByAsync(message.Username);
        user.IsGlobalSatisfactionReminderActive = !user.IsGlobalSatisfactionReminderActive;
        await _userRepository.UpdateAsync(user);
        await _userRepository.CommitAsync();

        CommandHandler.RemoveCommand(message.Username);

        _text = $"{(user.IsGlobalSatisfactionReminderActive ? "فعال‌سازی" : "غیرفعال‌سازی")} با موفقت انجام شد.👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
