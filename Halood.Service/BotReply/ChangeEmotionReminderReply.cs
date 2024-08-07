using Halood.Common;
using Halood.Domain.Dtos;
using Halood.Domain.Entities;
using Halood.Domain.Enums;
using Halood.Domain.Interfaces.BotAction;
using Halood.Domain.Interfaces.User;
using Halood.Domain.Interfaces.UserEmotionReminder;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Service.BotReply;

public class ChangeEmotionReminderReply : IBotReply
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<ChangeEmotionReminderReply> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IUserEmotionReminderRepository _userEmotionReminderRepository;
    private string _text = string.Empty;

    public ChangeEmotionReminderReply(ITelegramBotClient botClient, IUserRepository userRepository, IUserEmotionReminderRepository userEmotionReminderRepository)
    {
        _botClient = botClient;
        _userRepository = userRepository;
        _userEmotionReminderRepository = userEmotionReminderRepository;
    }

    public async Task ExecuteAsync(BotCommandMessage message, CancellationToken cancellationToken)
    {
        try
        {
            var givenText = message.Text.Split(" ")[1];
            if (givenText != EmotionReminder.Submit.ToString())
            {
                CommandHandler.ChangeEmotionReminder(message.Username, (int) ConvertTextToEmotionReminder(givenText));
                var selectedReminders = CommandHandler.GetEmotionReminders(message.Username);
                var inlineKeyboard = InitializeInlineKeyboard();

                foreach (var selectedReminder in selectedReminders)
                {
                    var reminderEnum = ConvertIntToEmotionReminder(selectedReminder.Item2);
                    foreach (var row in inlineKeyboard.InlineKeyboard)
                    {
                        foreach (var item in row)
                        {
                            if(item.CallbackData == reminderEnum.GetRoute())
                            {
                                item.Text = $"{(int) reminderEnum}:00 ✅";
                                break;
                            }
                        }
                    }
                }

                _text = $"در صورتی که زمان‌های مدنظر را انتخاب کرده‌اید، دکمه \"ثبت\" را بزنید.\n" +
                        $"در صورتی که مایل به انتخاب زمان‌های دیگر هستید، آن‌ها را انتخاب کنید.\n";
                await _botClient.SendTextMessageAsync(
                    chatId: message.ChatId,
                    text: _text,
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);

            }
        
            else
            {
                var user = await _userRepository.GetByAsync(message.Username);
                var reminders = CommandHandler.GetEmotionReminders(message.Username);
                await _userEmotionReminderRepository.DisableValidRemindersAsync(user.Id);
                foreach (var reminder in reminders)
                {
                    await _userEmotionReminderRepository.SaveAsync(new UserEmotionReminder
                    {
                        UserId = user.Id,
                        Hour = reminder.Item2
                    });
                }

                await _userEmotionReminderRepository.CommitAsync();

                _text = "تنظیمات با موفقیت انجام شد. 👍";
                await _botClient.SendTextMessageAsync(
                    chatId: message.ChatId,
                    text: _text,
                    cancellationToken: cancellationToken);

                CommandHandler.RemoveEmotionReminders(message.Username);
            }
        }
        catch (Exception ex)
        {
            _text = "تنظیمات با موفقیت انجام شد. 👍";
            await _botClient.SendTextMessageAsync(
                chatId: message.ChatId,
                text: _text,
                cancellationToken: cancellationToken);
        }
    }

    private EmotionReminder ConvertTextToEmotionReminder(string text)
    {
        return Enum.GetValues<EmotionReminder>().FirstOrDefault(emotion => emotion.GetDescription() == text);
    }

    private EmotionReminder ConvertIntToEmotionReminder(int hour)
    {
        foreach (var emotionReminder in Enum.GetValues<EmotionReminder>())
        {
            if((int)emotionReminder == hour)
                return emotionReminder;
        }

        return EmotionReminder.Submit;
    }

    private InlineKeyboardMarkup InitializeInlineKeyboard()
    {
        return new InlineKeyboardMarkup(new List<IEnumerable<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("7:00 🕖", EmotionReminder.Seven.GetRoute()),
                InlineKeyboardButton.WithCallbackData("9:00 🕘", EmotionReminder.Nine.GetRoute()),
                InlineKeyboardButton.WithCallbackData("11:00 🕚", EmotionReminder.Eleven.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("13:00 🕐", EmotionReminder.Thirteen.GetRoute()),
                InlineKeyboardButton.WithCallbackData("15:00 🕒", EmotionReminder.Fifteen.GetRoute()),
                InlineKeyboardButton.WithCallbackData("17:00 🕔", EmotionReminder.Seventeen.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("19:00 🕖", EmotionReminder.Nineteen.GetRoute()),
                InlineKeyboardButton.WithCallbackData("21:00 🕘", EmotionReminder.TwentyOne.GetRoute()),
                InlineKeyboardButton.WithCallbackData("23:00 🕚", EmotionReminder.TwentyThree.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("ثبت 👍", EmotionReminder.Submit.GetRoute()),
            }
        });
    }
}
