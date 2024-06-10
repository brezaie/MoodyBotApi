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


        public static InlineKeyboardMarkup MenuInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("شروع", CommandType.Start.GetDescription()),
                InlineKeyboardButton.WithCallbackData("ثبت رضایت",
                    CommandType.Satisfaction.GetDescription()),
                InlineKeyboardButton.WithCallbackData("ثبت احساس", CommandType.Feeling.GetDescription()),
                InlineKeyboardButton.WithCallbackData("تنظیم یادآور", CommandType.ToggleReminder.GetDescription()),
            });

        public static InlineKeyboardMarkup ReminderToggleInlineKeyboardMarkup = new InlineKeyboardMarkup(
            new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("بله", YesNoResponse.Yes.GetDescription()),
                InlineKeyboardButton.WithCallbackData("خیر", YesNoResponse.No.GetDescription()),
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
