using Halood.Domain.Dtos;
using Halood.Domain.Interfaces.BotAction;
using Telegram.Bot;

namespace Halood.Service.BotReply;

public class RecordThoughtReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private string _text = string.Empty;

    public RecordThoughtReply(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
