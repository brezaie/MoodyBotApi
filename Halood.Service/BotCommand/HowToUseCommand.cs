using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Halood.Service.BotCommand;

public class HowToUseCommand : IBotCommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<StartCommand> _logger;
    private string _text = string.Empty;

    public HowToUseCommand(ITelegramBotClient botClient, ILogger<StartCommand> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        await _botClient.SendChatActionAsync(
            chatId: message.ChatId,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);

        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
