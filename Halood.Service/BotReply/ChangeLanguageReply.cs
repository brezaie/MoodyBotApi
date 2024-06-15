using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Service.BotCommand;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotReply;

public class ChangeLanguageReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = string.Empty;

    public ChangeLanguageReply(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task Execute(BotCommandMessage message, CancellationToken cancellationToken)
    {
        // اگر متن وارد شده، هیچ یک از گزینه های پیشنهادی نبود
        if (((Language[])Enum.GetValues(typeof(Language))).All(x =>
                x.GetDescription() != message.Text))
        {
            _text = $"زبان انتخاب شده نادرست می‌باشد. لطفاً یکی از زبان‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: CommandHandler.LanguageInlineKeyboardMarkup,
                cancellationToken: cancellationToken);
            return;
        }

        CommandHandler.RemoveCommand(message.Username);

        _text = $"در حال حاضر فقط از زبان فارسی پشتیبانی می‌شود. زبان انگلیسی به زودی اضافه خواهد شد.";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
