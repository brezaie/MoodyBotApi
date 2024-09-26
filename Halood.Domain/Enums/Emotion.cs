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
    Anger = 6,

    [Description("آرامش")]
    [Route("/record_emotion_reply Serenity")]
    [Color("#FEDE6F")]
    Serenity = 7,

    [Description("وجد")]
    [Route("/record_emotion_reply Ecstacy")]
    [Color("#FFC50C")]
    Ecstacy = 3,

    [Description("عشق")]
    [Route("/record_emotion_reply Love")]
    [Color("#F3F2B2")]
    Love = 4,

    [Description("پذیرش")]
    [Route("/record_emotion_reply Acceptance")]
    [Color("#C7C88F")]
    Acceptance = 5,

    [Description("اعتماد")]
    [Route("/record_emotion_reply Trust")]
    [Color("#B4B46A")]
    Trust = 6,

    [Description("تحسین")]
    [Route("/record_emotion_reply Admiration")]
    [Color("#A2A346")]
    Admiration = 7,

    [Description("سلطه پذیری")]
    [Route("/record_emotion_reply Submission")]
    [Color("#DCECBD")]
    Submission = 8,

    [Description("دلهره")]
    [Route("/record_emotion_reply Apprehension")]
    [Color("#81AF81")]
    Apprehension = 9,

    [Description("وحشت")]
    [Route("/record_emotion_reply Terror")]
    [Color("#4C8D4D")]
    Terror = 10,

    [Description("حیرت")]
    [Route("/record_emotion_reply Awe")]
    [Color("#CFE5BF")]
    Awe = 11,

    [Description("حواس پرتی")]
    [Route("/record_emotion_reply Distraction")]
    [Color("#789DA3")]
    Distraction = 12,

    [Description("شگفتی")]
    [Route("/record_emotion_reply Amazement")]
    [Color("#3E727D")]
    Amazement = 13,

    [Description("مخالفت")]
    [Route("/record_emotion_reply Disapproval")]
    [Color("#C3DFE2")]
    Disapproval = 14,

    [Description("پکری")]
    [Route("/record_emotion_reply Pensiveness")]
    [Color("#778CB9")]
    Pensiveness = 15,

    [Description("سوگ")]
    [Route("/record_emotion_reply Grief")]
    [Color("#3D5B99")]
    Grief = 16,

    [Description("پشیمانی")]
    [Route("/record_emotion_reply Remorse")]
    [Color("#C3C8E5")]
    Remorse = 17,

    [Description("ملال")]
    [Route("/record_emotion_reply Boredom")]
    [Color("#A285B1")]
    Boredom = 18,

    [Description("انزجار")]
    [Route("/record_emotion_reply Loathing")]
    [Color("#7A508F")]
    Loathing = 19,

    [Description("تحقیر")]
    [Route("/record_emotion_reply Contempt")]
    [Color("#DFB4D2")]
    Contempt = 20,

    [Description("رنج")]
    [Route("/record_emotion_reply Annoyance")]
    [Color("#FF894C")]
    Annoyance = 21,

    [Description("جنون")]
    [Route("/record_emotion_reply Rage")]
    [Color("#F46322")]
    Rage = 22,

    [Description("پرخاشگری")]
    [Route("/record_emotion_reply Aggressiveness")]
    [Color("#FAC5BD")]
    Aggressiveness = 23,

    [Description("دلبستگی")]
    [Route("/record_emotion_reply Interest")]
    [Color("#FEB04C")]
    Interest = 24,

    [Description("انتظار")]
    [Route("/record_emotion_reply Anticipation")]
    [Color("#FF9818")]
    Anticipation = 35,

    [Description("هشیاری")]
    [Route("/record_emotion_reply Vigilance")]
    [Color("#FF8B00")]
    Vigilance = 26,

    [Description("خوشبینی")]
    [Route("/record_emotion_reply Optimism")]
    [Color("#FCE9AE")]
    Optimism = 27
}
