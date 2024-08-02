using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotion;
using Halood.Domain.Interfaces.UserSatisfaction;
using Halood.Service.BotCommand;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Halood.Service.BotReply;

public class HowDoYouFeelReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<NoCommand> _logger;
    private string _text = string.Empty;
    private readonly IUserRepository _userRepository;
    private readonly IUserEmotionRepository _userEmotionRepository;

    public HowDoYouFeelReply(ITelegramBotClient botClient, IUserRepository userRepository, IUserEmotionRepository userEmotionRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
        _userEmotionRepository = userEmotionRepository;
    }

    public async Task Execute(BotCommandMessage message, CancellationToken cancellationToken)
    {
        // اگر متن وارد شده، هیچ یک از احساس های پیشنهادی نبود
        if (((Emotion[])Enum.GetValues(typeof(Emotion))).All(x =>
                x.ToString() != message.Text))
        {
            _text = $"احساس انتخاب شده نادرست می‌باشد. لطفاً یکی از احساس‌های پیشنهادی را انتخاب کنید.";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                replyMarkup: CommandHandler.EmotionInlineKeyboardMarkup,
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
                x.ToString() == message.Text);

        await _userEmotionRepository.SaveAsync(new UserEmotion
        {
            UserId = userId,
            RegistrationDate = message.Date,
            EmotionText = emotion.ToString()

        });

        await _userEmotionRepository.CommitAsync();

        CommandHandler.RemoveCommand(message.Username);

        _text = $"احساس \"{emotion.GetDescription()}\" برای این لحظه‌تان با موفقبت ثبت شد.  👍";
        await _botClient.SendTextMessageAsync(
            chatId: message.ChatId,
            text: _text,
            cancellationToken: cancellationToken);
    }
}
