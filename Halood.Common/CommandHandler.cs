using Halood.Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Common
{
    public class CommandHandler
    {
        private static Dictionary<string, CommandType> _commands;

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

        public static InlineKeyboardMarkup ReminderToggleInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("بله 👍", YesNoResponse.Yes.GetDescription()),
                InlineKeyboardButton.WithCallbackData("خیر 👎", YesNoResponse.No.GetDescription()),
            });

        public static InlineKeyboardMarkup LanguageInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("فارسی 🇮🇷", Language.Persian.GetDescription()),
                InlineKeyboardButton.WithCallbackData("English 🇬🇧", Language.English.GetDescription()), 
            });

        public static InlineKeyboardMarkup SettingsInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("یادآور ⏰", CommandType.Reminder.GetDescription()),
                InlineKeyboardButton.WithCallbackData("زبان 🌎", CommandType.Language.GetDescription())
            });

        public static void AddCommand(string username, CommandType commandType)
        {
            if(_commands == null)
                _commands = new Dictionary<string, CommandType>();

            var doesCommandExist = _commands.FirstOrDefault(x => x.Key == username);
            if (doesCommandExist.Value != commandType)
            {
                _commands.Add(username, commandType);
            }
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

    }
}
