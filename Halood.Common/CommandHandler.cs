using Halood.Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Common
{
    public class CommandHandler
    {
        private static Dictionary<string, CommandType> _commands;
        private static List<(string Username, int Hour)> _emotionReminders;

        public static List<string> SpecialUserNames = new()
        {
            "brezaie"
        };

        public static ReplyKeyboardMarkup SatisfactionLevelReplyKeyboardMarkup = new(
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


        public static InlineKeyboardMarkup SatisfactionLevelInlineKeyboardMarkup =
        new(new List<IEnumerable<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("خیلی زیاد 😍", SatisfactionLevel.Perfect.GetDescription()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("زیاد 😊", SatisfactionLevel.Good.GetDescription()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("متوسط 😏", SatisfactionLevel.SoSo.GetDescription()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("کم 😞", SatisfactionLevel.Bad.GetDescription()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("خیلی کم 😥", SatisfactionLevel.Awful.GetDescription()),
            }
        });

        public static ReplyKeyboardMarkup EmotionReplyKeyboardMarkup = new(
            new[]
            {
                new KeyboardButton[]
                {
                    Emotion.Serenity.GetDescription(),
                    Emotion.Joy.GetDescription(),
                    Emotion.Ecstacy.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Love.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Acceptance.GetDescription(),
                    Emotion.Trust.GetDescription(),
                    Emotion.Admiration.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Submission.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Apprehension.GetDescription(),
                    Emotion.Fear.GetDescription(),
                    Emotion.Terror.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Awe.GetDescription()
                },

                new KeyboardButton[]
                {
                    Emotion.Distraction.GetDescription(),
                    Emotion.Surprise.GetDescription(),
                    Emotion.Amazement.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Disapproval.GetDescription()
                },

                new KeyboardButton[]
                {
                    Emotion.Pensiveness.GetDescription(),
                    Emotion.Sadness.GetDescription(),
                    Emotion.Grief.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Remorse.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Boredom.GetDescription(),
                    Emotion.Disgust.GetDescription(),
                    Emotion.Loathing.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Contempt.GetDescription()
                },


                new KeyboardButton[]
                {
                    Emotion.Annoyance.GetDescription(),
                    Emotion.Anger.GetDescription(),
                    Emotion.Rage.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Aggressiveness.GetDescription()
                },

                new KeyboardButton[]
                {
                    Emotion.Interest.GetDescription(),
                    Emotion.Anticipation.GetDescription(),
                    Emotion.Vigilance.GetDescription()
                },
                new KeyboardButton[]
                {
                    Emotion.Optimism.GetDescription()
                },
            }
        )
        {
            ResizeKeyboard = true
        };

        public static InlineKeyboardMarkup MenuInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("شروع", CommandType.Start.GetDescription()),
                InlineKeyboardButton.WithCallbackData("ثبت رضایت",
                    CommandType.Satisfaction.GetDescription()),
                InlineKeyboardButton.WithCallbackData("ثبت احساس", CommandType.Emotion.GetDescription()),
                InlineKeyboardButton.WithCallbackData("تنظیمات", CommandType.Settings.GetDescription()),
            });

        public static InlineKeyboardMarkup SatisfactionReminderToggleInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("بله 👍", YesNoResponse.Yes.GetDescription()),
                InlineKeyboardButton.WithCallbackData("خیر 👎", YesNoResponse.No.GetDescription()),
            });

        public static InlineKeyboardMarkup EmotionReminderInlineKeyboardMarkup =
            new(new List<IEnumerable<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData("7:00 🕖", EmotionReminder.Seven.GetDescription()),
                    InlineKeyboardButton.WithCallbackData("9:00 🕘", EmotionReminder.Nine.GetDescription()),
                    InlineKeyboardButton.WithCallbackData("11:00 🕚", EmotionReminder.Eleven.GetDescription()),
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData("13:00 🕐", EmotionReminder.Thirteen.GetDescription()),
                    InlineKeyboardButton.WithCallbackData("15:00 🕒", EmotionReminder.Fifteen.GetDescription()),
                    InlineKeyboardButton.WithCallbackData("17:00 🕔", EmotionReminder.Seventeen.GetDescription()),
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData("19:00 🕖", EmotionReminder.Nineteen.GetDescription()),
                    InlineKeyboardButton.WithCallbackData("21:00 🕘", EmotionReminder.TwentyOne.GetDescription()),
                    InlineKeyboardButton.WithCallbackData("23:00 🕚", EmotionReminder.TwentyThree.GetDescription()),
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData("ثبت 👍", EmotionReminder.Submit.GetDescription()),

                }
            });

    public static InlineKeyboardMarkup LanguageInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("فارسی 🇮🇷", Language.Persian.GetDescription()),
                InlineKeyboardButton.WithCallbackData("English 🇬🇧", Language.English.GetDescription()), 
            });
        
        public static InlineKeyboardMarkup SettingsInlineKeyboardMarkup = new(new List<IEnumerable<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("یادآور رضایت از زندگی 🕙", CommandType.SatisfactionReminder.GetDescription()),
                InlineKeyboardButton.WithCallbackData("یادآور احساس‌ها ⏰", CommandType.EmotionReminder.GetDescription())
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("زبان 🌎", CommandType.Language.GetDescription()),
            }
        });

        public static void AddCommand(string username, CommandType commandType)
        {
            if(_commands == null)
                _commands = new Dictionary<string, CommandType>();

            RemoveCommand(username);
            _commands.Add(username, commandType);
        }

        public static void RemoveCommand(string username)
        {
            if(_commands != null)
                _commands.Remove(username);
        }

        public static CommandType GetCommand(string username)
        {
            if (_commands == null)
                return CommandType.Unknown;

            var command = _commands.FirstOrDefault(x => x.Key == username);
            return command.Value;
        }


        public static void ChangeEmotionReminder(string username, int hour)
        {
            if (_emotionReminders == null)
                _emotionReminders = new List<(string Username, int Hour)>();

            var doesEmotionReminderExist = _emotionReminders.FirstOrDefault(x => x.Username == username && x.Hour == hour);
            if (!string.IsNullOrEmpty(doesEmotionReminderExist.Username))
                _emotionReminders.Remove(doesEmotionReminderExist);
            else
                _emotionReminders.Add((username, hour));
        }

        public static void RemoveEmotionReminders(string username)
        {
            if (_emotionReminders != null)
                _emotionReminders.RemoveAll(x => x.Username == username);
        }

        public static List<(string, int)> GetEmotionReminders(string username)
        {
            if (_emotionReminders == null)
                return new List<(string, int)>();

            return _emotionReminders.Where(x => x.Username == username).ToList();
        }

    }
}
