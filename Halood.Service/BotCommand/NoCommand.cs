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
    private readonly IBotReply _unkownReply;
    private readonly IBotReply _howIsYourSatisfactionReply;
    private readonly IBotReply _howDoYouFeelReply;
    private readonly IBotReply _toggleSatisfactionReminderReply;
    private readonly IBotReply _changeLanguageReply;
    private readonly IBotReply _changeEmotionReminder;

    delegate Task DoAction(BotCommandMessage message, CancellationToken cancellationToken);

    //private Dictionary<CommandType, DoAction> replyActions = new ();
    private Dictionary<CommandType, IBotReply> replyActions = new();

    public NoCommand(IEnumerable<IBotReply> botReplies)
    {
        _unkownReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(UnknownReply));
        _howIsYourSatisfactionReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(RecordSatisfactionReply));
        _howDoYouFeelReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(RecordEmotionReply));
        _toggleSatisfactionReminderReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(ToggleReminderReply));
        _changeLanguageReply = botReplies.FirstOrDefault(x => x.GetType() == typeof(ChangeLanguageReply));
        _changeEmotionReminder = botReplies.FirstOrDefault(x => x.GetType() == typeof(ChangeEmotionReminderReply));

        replyActions.Add(CommandType.Unknown, _unkownReply);
        replyActions.Add(CommandType.Satisfaction, _howIsYourSatisfactionReply);
        replyActions.Add(CommandType.Emotion, _howDoYouFeelReply);
        replyActions.Add(CommandType.SatisfactionReminder, _toggleSatisfactionReminderReply);
        replyActions.Add(CommandType.Language, _changeLanguageReply);
        replyActions.Add(CommandType.EmotionReminder, _changeEmotionReminder);
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        var previousCommand = CommandHandler.GetCommand(message.Username);
        await replyActions[previousCommand].ExecuteAsync(message, cancellationToken);
    }
}
