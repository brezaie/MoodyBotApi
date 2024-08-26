using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Service.BotReply;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotCommand;

public class NoCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private string _text = string.Empty;
    public NoCommand(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        _text = message.Text is not { } messageText ? $"متنی برای پردازش ارسال نشده است" : $"دستور ارسالی صحیح نمیباشد";

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
