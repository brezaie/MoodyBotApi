using Halood.Domain.Interfaces.User;
using System.Threading;
using Halood.Common;
using Halood.Domain.Enums;
using Telegram.Bot.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Examples.WebHook.Jobs;

public class ReminderJob : IJob
{
    private readonly IUserRepository _userRepository;
    private readonly ITelegramBotClient _botClient;

    public ReminderJob(IUserRepository userRepository, ITelegramBotClient botClient)
    {
        _userRepository = userRepository;
        _botClient = botClient;
    }
    
    public async Task Run()
    {
        var users = await _userRepository.GetAllAsync();
        foreach (var user in users.Where(x => x.ChatId > 0))
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
                {
                    new KeyboardButton[]
                    {
                        SatisfactionLevel.Awful.GetDescription(),
                        SatisfactionLevel.Bad.GetDescription(),
                        SatisfactionLevel.SoSo.GetDescription(),
                        SatisfactionLevel.Good.GetDescription(),
                        SatisfactionLevel.Perfect.GetDescription()
                    }
                })
            {
                ResizeKeyboard = true
            };

            CommandHandler.AddCommand(user.Username, CommandType.Satisfaction);

            await _botClient.SendTextMessageAsync(
                chatId: user.ChatId,
                text: "چه عددی به رضایت از زندگی امروزت میدی؟ هر چی عدد بالاتری انتخاب کنی، یعنی رضایت بیشتری داری",
            replyMarkup: replyKeyboardMarkup);
        }
    }
}
