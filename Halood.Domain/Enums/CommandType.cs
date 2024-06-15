using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum CommandType
{
    Unknown = 0,

    [Description("/start")]
    Start = 1,

    [Description("/how_is_your_satisfaction")]
    Satisfaction = 2,

    [Description("/how_do_you_feel")]
    Emotion = 3,

    [Description("/change_settings")]
    Settings = 4,

    [Description("/toggle_reminder")]
    Reminder = 5,

    [Description("/change_language")]
    Language = 6
}
