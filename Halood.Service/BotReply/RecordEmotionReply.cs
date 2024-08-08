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
        var emotionsList = CommandHandler.GetEmotionInlineKeyboardMarkup();
        var givenEmotion = message.Text.Split(" ")[1];
        
        var emotion = ((Emotion[]) Enum.GetValues(typeof(Emotion)))
            .FirstOrDefault(x =>
                x.ToString() == givenEmotion);

        var userId = (await _userRepository.GetByAsync(message.Username)).Id;
        await _userEmotionRepository.SaveAsync(new UserEmotion
        {
            UserId = userId,
            RegistrationDate = message.Date,
            EmotionText = emotion.ToString()

        });

        await _userEmotionRepository.CommitAsync();

        foreach (var satLevel in emotionsList.InlineKeyboard)
        {
            foreach (var row in satLevel)
            {
                if (row.Text != emotion.GetDescription()) continue;

                row.Text = $"{row.Text} ✅";
                break;
            }
        }

        _text = $"احساس \"{emotion.GetDescription()}\" برای این لحظه‌تان با موفقبت ثبت شد.  👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            replyMarkup:emotionsList,
            cancellationToken: cancellationToken);
    }
}
