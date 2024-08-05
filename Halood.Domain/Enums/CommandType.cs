using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum CommandType
{
    Unknown = 0,

    [Description("/start")]
    Start = 1,

    [Description("/record_satisfaction")]
    Satisfaction = 2,

    [Description("/record_emotion")]
    Emotion = 3,

    [Description("/change_settings")]
    Settings = 4,

    [Description("/toggle_satisfaction_reminder")]
    SatisfactionReminder = 5,

    [Description("/change_language")]
    Language = 6,

    [Description("/generate_report")]
    Report = 7,

    [Description("/change_emotion_reminder")]
    EmotionReminder = 8,

    [Description("/record_satisfaction_reply")]
    SatisfactionReply = 9,

    [Description("/record_emotion_reply")]
    EmotionReply = 10
}
