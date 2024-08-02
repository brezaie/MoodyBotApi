using Halood.Common;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotionReminder;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.WebHook.Jobs;

public class EmotionReminderJob : IJob
{
    private readonly IUserEmotionReminderRepository _userEmotionReminderRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _botClient;

    public EmotionReminderJob(ITelegramBotClient botClient, IUserEmotionReminderRepository userEmotionReminderRepository, IUserRepository userRepository)
    {
        _botClient = botClient;
        _userEmotionReminderRepository = userEmotionReminderRepository;
        _userRepository = userRepository;
    }

    public async Task Run()
    {
        var text = "این پیام از طریق یادآورِ بات برای شما ارسال شده است.\n\n" +
                   $"کدام‌یک از احساس‌های زیر به احساسی که در این لحظه تجربه می‌کنید، نزدیک‌تر است؟\n\n" +
                   $"برای تغییر زمان‌های یادآور و یا لغو آن، به بخش \"تغییر تنظیمات -> یادآور احساس‌ها\" مراجعه کنید.";

        var iranTime = DateTimeOffset.UtcNow.AddHours(3).AddMinutes(30);

        // This checking is because of the job which runs every 30 mins. The reason job runs every 30 mins, is because of the time zone difference by 30 mins.
        if(iranTime.Minute > 15)    return;

        var reminders = await _userEmotionReminderRepository.GetValidUserEmotionRemindersAsync();
        foreach (var reminder in reminders.Where(x => x.Hour == iranTime.Hour))
        {
            var user = await _userRepository.GetAsync(reminder.UserId);
            CommandHandler.AddCommand(user.Username, CommandType.Emotion);

            await _botClient.SendTextMessageAsync(
                chatId: user.ChatId,
                text: text,
                replyMarkup: CommandHandler.EmotionReminderInlineKeyboardMarkup);
        }
    }
}
