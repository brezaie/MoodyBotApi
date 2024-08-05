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

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        var givenLanguage = message.Text.Split(" ")[1];
        // اگر متن وارد شده، هیچ یک از گزینه های پیشنهادی نبود
        if (((Language[])Enum.GetValues(typeof(Language))).All(x =>
                x.GetDescription() != givenLanguage))
        {
            _text = $"زبان انتخاب شده نادرست می‌باشد. لطفاً یکی از زبان‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: CommandHandler.LanguageInlineKeyboardMarkup,
                cancellationToken: cancellationToken);
            return;
        }

        _text = givenLanguage == Language.Persian.GetDescription()
            ? $"زبان فارسی با موفقیت اعمال شد."
            : $"در حال حاضر فقط از زبان فارسی پشتیبانی می‌شود. زبان انگلیسی به زودی اضافه خواهد شد.";

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
