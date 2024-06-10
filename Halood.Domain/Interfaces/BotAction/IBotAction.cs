using Halood.Domain.Dtos;
using Telegram.Bot.Types;

namespace Halood.Domain.Interfaces.BotAction;

public interface IBotAction
{
    Task Execute(BotActionMessage message, CancellationToken cancellationToken);
}
