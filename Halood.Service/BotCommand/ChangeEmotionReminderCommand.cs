using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class ChangeEmotionReminderCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<ChangeEmotionReminderCommand> _logger;
    private string _text = string.Empty;

    public ChangeEmotionReminderCommand(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        CommandHandler.AddCommand(message.Username, CommandType.EmotionReminder);

        _text =
            $"چند گزینه بعنوان ساعت‌هایی که یادآور ثبت احساس برای شما ارسال شود را ثبت کنید و سپس دکمه \"ثبت\" را بزنید.\n" +
            $"در صورتی که تمایل به ارسال یادآور ندارید، بدون انتخاب هیچ گزینه‌ای، دکمه \"ثبت\" را بزنید.\n";

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: CommandHandler.EmotionReminderInlineKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}
