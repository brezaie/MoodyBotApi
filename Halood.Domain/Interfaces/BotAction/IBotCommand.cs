using Halood.Domain.Dtos;
using Telegram.Bot.Types;

namespace Halood.Domain.Interfaces.BotAction;

public interface IBotCommand
{
    Task Execute(BotCommandMessage message, CancellationToken cancellationToken);
}
