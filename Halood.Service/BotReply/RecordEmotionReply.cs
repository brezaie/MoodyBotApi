using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Service.BotCommand;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Service.BotReply;

public class RecordEmotionReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = string.Empty;
    private readonly IUserRepository _userRepository;
    private readonly IUserEmotionRepository _userEmotionRepository;

    public RecordEmotionReply(ITelegramBotClient botClient, IUserRepository userRepository, IUserEmotionRepository userEmotionRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
        _userEmotionRepository = userEmotionRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        var givenEmotion = message.Text.Split(" ")[1];
        if (givenEmotion == CommandHandler.MoreEmotionsRoute.Split(" ")[1])
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: $"کدام‌یک از احساس‌های زیر به احساسی که در این لحظه تجربه می‌کنید، نزدیک‌تر است؟\n\n",
                replyMarkup: CommandHandler.GetMoreEmotionsInlineKeyboardMarkup(),
                cancellationToken: cancellationToken);
            return;
        }
        if (givenEmotion == CommandHandler.LessEmotionsRoute.Split(" ")[1])
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: $"کدام‌یک از احساس‌های زیر به احساسی که در این لحظه تجربه می‌کنید، نزدیک‌تر است؟\n\n",
                replyMarkup: CommandHandler.GetBasicEmotionsInlineKeyboardMarkup(),
                cancellationToken: cancellationToken);
            return;
        }


        var emotion = ((Emotion[]) Enum.GetValues(typeof(Emotion)))
            .FirstOrDefault(x =>
                x.ToString() == givenEmotion);

        var basicEmotionsList = CommandHandler.GetBasicEmotionsInlineKeyboardMarkup();
        var moreEmotionsList = CommandHandler.GetMoreEmotionsInlineKeyboardMarkup();
        var userId = (await _userRepository.GetByAsync(message.Username)).Id;
        await _userEmotionRepository.SaveAsync(new UserEmotion
        {
            UserId = userId,
            RegistrationDate = message.Date,
            EmotionText = emotion.ToString()

        });

        await _userEmotionRepository.CommitAsync();

        InlineKeyboardButton reply = null;

        foreach (var satLevel in basicEmotionsList.InlineKeyboard)
        {
            foreach (var row in satLevel)
            {
                if (row.Text != emotion.GetDescription()) continue;

                row.Text = $"{row.Text} ✅";
                reply = row;
                break;
            }
        }

        if(reply == null)
            foreach (var satLevel in moreEmotionsList.InlineKeyboard)
            {
                foreach (var row in satLevel)
                {
                    if (row.Text != emotion.GetDescription()) continue;

                    row.Text = $"{row.Text} ✅";
                    reply = row;
                    break;
                }
            }

        _text = $"احساس \"{emotion.GetDescription()}\" برای این لحظه‌تان با موفقبت ثبت شد.  👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup: new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    reply
                }
            }),
            cancellationToken: cancellationToken);
    }
}
