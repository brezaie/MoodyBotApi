using Telegram.Bot.Types;

namespace Halood.Domain.Interfaces;

public interface IBotAction
{
    Task Execute(Message message, CancellationToken cancellationToken);
}
