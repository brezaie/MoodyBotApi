using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum EmotionReminder
{
    Unknown = 0,
    [Description("Seven")]
    [Route("/change_emotion_reminder_reply Seven")]
    Seven = 7,

    [Description("Nine")]
    [Route("/change_emotion_reminder_reply Nine")]
    Nine = 9,

    [Description("Eleven")]
    [Route("/change_emotion_reminder_reply Eleven")]
    Eleven = 11,

    [Description("Thirteen")]
    [Route("/change_emotion_reminder_reply Thirteen")]
    Thirteen = 13,

    [Description("Fifteen")]
    [Route("/change_emotion_reminder_reply Fifteen")]
    Fifteen = 15,

    [Description("Seventeen")]
    [Route("/change_emotion_reminder_reply Seventeen")] Seventeen = 17,

    [Description("Nineteen")]
    [Route("/change_emotion_reminder_reply Nineteen")]
    Nineteen = 19,

    [Description("TwentyOne")]
    [Route("/change_emotion_reminder_reply TwentyOne")]
    TwentyOne = 21,

    [Description("TwentyThree")]
    [Route("/change_emotion_reminder_reply TwentyThree")]
    TwentyThree = 23,

    [Description("Submit")]
    [Route("/change_emotion_reminder_reply Submit")]
    Submit = 25
}
