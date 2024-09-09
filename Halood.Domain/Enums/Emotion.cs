using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum Emotion
{
    Unknown = 0,

    [Description("شادی")]
    [Route("/record_emotion_reply Happiness")]
    [Color("#FFD23E")]
    Happiness = 1,
    

    [Description("ترس")]
    [Route("/record_emotion_reply Fear")]
    [Color("#5E985E")]
    Fear = 2,

    [Description("تعجب")]
    [Route("/record_emotion_reply Surprise")]
    [Color("#518289")]
    Surprise = 3,

    [Description("ناراحتی")]
    [Route("/record_emotion_reply Sadness")]
    [Color("#778CB9")]
    Sadness = 4,

    [Description("تنفر")]
    [Route("/record_emotion_reply Disgust")]
    [Color("#876298")]
    Disgust = 5,

    [Description("خشم")]
    [Route("/record_emotion_reply Anger")]
    [Color("#FF7733")]
    Anger = 6
}
