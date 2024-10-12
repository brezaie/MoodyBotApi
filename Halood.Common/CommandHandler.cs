using Halood.Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Common
{
    public class CommandHandler
    {
        private static Dictionary<string, CommandType> _commands;
        private static List<(string Username, int Hour)> _emotionReminders;
        public static string MoreEmotionsText = "احساس‌های بیش‌تر";
        public static string MoreEmotionsRoute = "/record_emotion_reply MoreEmotions";
        public static string LessEmotionsText = "لیست قبلی احساس‌ها";
        public static string LessEmotionsRoute = "/record_emotion_reply LessEmotions";


        public static Dictionary<string, long> SpecialUserNames = new()
        {
            {"brezaie", 92700050}
        };

        public static InlineKeyboardMarkup GetSatisfactionLevelInlineKeyboardMarkup() => 
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


        public static InlineKeyboardMarkup GetBasicEmotionsInlineKeyboardMarkup() =>
            new(new List<IEnumerable<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Emotion.Happiness.GetDescription(),
                        Emotion.Happiness.GetRoute()),
                    InlineKeyboardButton.WithCallbackData(Emotion.Fear.GetDescription(), Emotion.Fear.GetRoute())
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Emotion.Surprise.GetDescription(),
                        Emotion.Surprise.GetRoute()),
                    InlineKeyboardButton.WithCallbackData(Emotion.Sadness.GetDescription(), Emotion.Sadness.GetRoute())

                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Emotion.Disgust.GetDescription(), Emotion.Disgust.GetRoute()),
                    InlineKeyboardButton.WithCallbackData(Emotion.Anger.GetDescription(), Emotion.Anger.GetRoute())
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(MoreEmotionsText, MoreEmotionsRoute)
                }
            });

        public static InlineKeyboardMarkup GetFaqInlineKeyboardMarkup() =>
            new(new List<IEnumerable<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Faq.WhatTheBotIsFor.GetDescription(), Faq.WhatTheBotIsFor.GetRoute()),
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Faq.HowToWorkWithBot.GetDescription(), Faq.HowToWorkWithBot.GetRoute())

                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Faq.HowToChangeReminders.GetDescription(), Faq.HowToChangeReminders.GetRoute())
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Faq.WhenWeeklyReportIsSent.GetDescription(), Faq.WhenWeeklyReportIsSent.GetRoute())
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(Faq.HowToReportProblem.GetDescription(), Faq.HowToReportProblem.GetRoute())
                }
            });

        public static InlineKeyboardMarkup GetMoreEmotionsInlineKeyboardMarkup()
        {
            var items = new List<IEnumerable<InlineKeyboardButton>>();
            var counter = 1;
            var coupleItems = new List<InlineKeyboardButton>();
            foreach (var emotion in Enum.GetValues<Emotion>().Where(x => (int) x > (int) Emotion.Anger))
            {
                coupleItems.Add(InlineKeyboardButton.WithCallbackData(emotion.GetDescription(), emotion.GetRoute()));
                counter++;
                if (counter % 2 == 0) continue;

                items.Add(coupleItems);
                coupleItems = new List<InlineKeyboardButton>();
            }

            items.Add(new List<InlineKeyboardButton>{ InlineKeyboardButton.WithCallbackData(LessEmotionsText, LessEmotionsRoute) });

            return new InlineKeyboardMarkup(items);
        }
            

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
                InlineKeyboardButton.WithCallbackData("بله 👍", YesNoResponse.Yes.GetRoute()),
                InlineKeyboardButton.WithCallbackData("خیر 👎", YesNoResponse.No.GetRoute()),
            });

        public static InlineKeyboardMarkup EmotionReminderInlineKeyboardMarkup =
            new(new List<IEnumerable<InlineKeyboardButton>>
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

    public static InlineKeyboardMarkup LanguageInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("فارسی 🇮🇷", Language.Persian.GetRoute()),
                InlineKeyboardButton.WithCallbackData("English 🇬🇧", Language.English.GetRoute())
            });
        
        public static InlineKeyboardMarkup SettingsInlineKeyboardMarkup = new(new List<IEnumerable<InlineKeyboardButton>>
        {
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("یادآور رضایت از زندگی 🕙", CommandType.SatisfactionReminder.GetRoute()),
                InlineKeyboardButton.WithCallbackData("یادآور احساس‌ها ⏰", CommandType.EmotionReminder.GetRoute())
            },
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("زبان 🌎", CommandType.Language.GetRoute()),
            }
        });
        
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
