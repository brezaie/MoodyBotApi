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
        
        public static InlineKeyboardMarkup SatisfactionLevelInlineKeyboardMarkup =
        new(new List<IEnumerable<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(SatisfactionLevel.Perfect.GetDescription(), SatisfactionLevel.Perfect.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(SatisfactionLevel.Good.GetDescription(), SatisfactionLevel.Good.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(SatisfactionLevel.SoSo.GetDescription(), SatisfactionLevel.SoSo.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(SatisfactionLevel.Bad.GetDescription(), SatisfactionLevel.Bad.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(SatisfactionLevel.Awful.GetDescription(), SatisfactionLevel.Awful.GetRoute()),
            }
        });

        public static InlineKeyboardMarkup EmotionInlineKeyboardMarkup =
        new(new List<IEnumerable<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Serenity.GetDescription(), Emotion.Serenity.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Joy.GetDescription(), Emotion.Joy.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Ecstacy.GetDescription(), Emotion.Ecstacy.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Love.GetDescription(), Emotion.Love.GetRoute())
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Acceptance.GetDescription(), Emotion.Acceptance.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Trust.GetDescription(), Emotion.Trust.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Admiration.GetDescription(), Emotion.Admiration.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Submission.GetDescription(), Emotion.Submission.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Apprehension.GetDescription(), Emotion.Apprehension.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Fear.GetDescription(), Emotion.Fear.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Terror.GetDescription(), Emotion.Terror.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Awe.GetDescription(), Emotion.Awe.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Distraction.GetDescription(), Emotion.Distraction.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Surprise.GetDescription(), Emotion.Surprise.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Amazement.GetDescription(), Emotion.Amazement.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Disapproval.GetDescription(), Emotion.Disapproval.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Pensiveness.GetDescription(), Emotion.Pensiveness.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Sadness.GetDescription(), Emotion.Sadness.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Grief.GetDescription(), Emotion.Grief.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Remorse.GetDescription(), Emotion.Remorse.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Boredom.GetDescription(), Emotion.Boredom.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Disgust.GetDescription(), Emotion.Disgust.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Loathing.GetDescription(), Emotion.Loathing.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Contempt.GetDescription(), Emotion.Contempt.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Annoyance.GetDescription(), Emotion.Annoyance.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Anger.GetDescription(), Emotion.Anger.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Rage.GetDescription(), Emotion.Rage.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Aggressiveness.GetDescription(), Emotion.Aggressiveness.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Interest.GetDescription(), Emotion.Interest.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Anticipation.GetDescription(), Emotion.Anticipation.GetRoute()),
                InlineKeyboardButton.WithCallbackData(Emotion.Vigilance.GetDescription(), Emotion.Vigilance.GetRoute()),
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(Emotion.Optimism.GetDescription(), Emotion.Optimism.GetRoute()),
            }
        });

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
