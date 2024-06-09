using Halood.Domain.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Halood.Common
{
    public class CommandHandler
    {
        private static Dictionary<string, CommandType> _commands;

        public static ReplyKeyboardMarkup satisfactionLevelReplyKeyboardMarkup = new(
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


        public static void AddCommand(string username, CommandType commandType)
        {
            var doesCommandExist = _commands.FirstOrDefault(x => x.Key == username);
            if (doesCommandExist.Value != commandType)
            {
                _commands.Add(username, commandType);
            }
        }

        public static void RemoveCommand(string username)
        {
            _commands.Remove(username);
        }
        
    }
}
