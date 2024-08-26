using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum CommandType
{
    Unknown = 0,

    [Description("/start")]
    [Route("/start")]
    Start = 1,

    [Description("/record_satisfaction")]
    [Route("/record_satisfaction")]
    Satisfaction = 2,

    [Description("/record_emotion")]
    [Route("/record_emotion")]
    Emotion = 3,

    [Description("/change_settings")]
    [Route("/change_settings")]
    Settings = 4,

    [Description("/toggle_satisfaction_reminder")]
    [Route("/toggle_satisfaction_reminder")]
    SatisfactionReminder = 5,

    [Description("/change_language")]
    [Route("/change_language")]
    Language = 6,

    [Description("/generate_report")]
    [Route("/generate_report")]
    Report = 7,

    [Description("/change_emotion_reminder")]
    [Route("/change_emotion_reminder")]
    EmotionReminder = 8,

    [Description("/record_satisfaction_reply")]
    [Route("/record_satisfaction_reply")]
    SatisfactionReply = 9,

    [Description("/record_emotion_reply")]
    [Route("/record_emotion_reply")]
    EmotionReply = 10,

    [Description("/change_language_reply")]
    [Route("/change_language_reply")]
    LanguageReply = 11,

    [Route("/change_emotion_reminder_reply")]
    EmotionReminderReply = 12,

    [Route("/toggle_satisfaction_reminder_reply")]
    SatisfactionReminderReply = 13,

    [Route("/record_thought")]
    RecordThoughtCommand = 14,

    [Route("/record_thought_reply")]
    RecordThoughtReply = 15
}
