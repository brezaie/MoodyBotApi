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
        // اگر متن وارد شده، هیچ یک از احساس های پیشنهادی نبود
        if (((Emotion[])Enum.GetValues(typeof(Emotion))).All(x =>
                x.ToString() != givenEmotion))
        {
            _text = $"احساس انتخاب شده نادرست می‌باشد. لطفاً یکی از احساس‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: emotionsList,
                cancellationToken: cancellationToken);
            return;
        }

        var userId = (await _userRepository.GetByAsync(message.Username)).Id;
        var lastUserEmotion = await _userEmotionRepository.GetLastUserEmotionAsync(userId);

        if (!CommandHandler.SpecialUserNames.Contains(message.Username) &&
            lastUserEmotion is not null &&
            (message.Date - lastUserEmotion.RegistrationDate).TotalMinutes <= 60)
        {
            _text =
                $"از آخرین دفعه که احساس خود را ثبت کرده‌اید، کم‌تر از 1 ساعت گذشته است. پس از گذشت این زمان می‌توانید مجدد احساس خود را ثبت کنید 🙂";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                cancellationToken: cancellationToken);
            return;
        }

        var emotion = ((Emotion[]) Enum.GetValues(typeof(Emotion)))
            .FirstOrDefault(x =>
                x.ToString() == givenEmotion);

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
