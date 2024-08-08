using Halood.Domain.Interfaces.User;
using Halood.Common;
using Halood.Domain.Enums;

namespace Telegram.Bot.Examples.WebHook.Jobs;

public class SatisfactionReminderJob : IJob
{
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _botClient;

    public SatisfactionReminderJob(IUserRepository userRepository, ITelegramBotClient botClient)
    {
        _userRepository = userRepository;
        _botClient = botClient;
    }
    
    public async Task Run()
    {
        var text = "این پیام از طریق یادآورِ بات برای شما ارسال شده است.\n\n" +
                   "کدام یک از گزینه‌های زیر، میزان رضایت شما از لحظه‌ای که در آن هستید را می‌تواند به بهترین شکل نشان دهد؟\n\n" +
                   $"برای لغو یادآور، به بخش \"تغییر تنظیمات -> یادآور رضایت از زندگی\" مراجعه کنید.";
        ;

        var users = await _userRepository.GetSatisfactionRemindersAsync();
        foreach (var user in users)
        {
            await _botClient.SendTextMessageAsync(
                chatId: user.ChatId,
                text: text,
                replyMarkup: CommandHandler.GetSatisfactionLevelInlineKeyboardMarkup());
        }
    }
}
