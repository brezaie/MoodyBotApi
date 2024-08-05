using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum Emotion
{
    Unknown = 0,

    [Description("آرامش")]
    [Route("/record_emotion_reply Serenity")]
    Serenity = 1,

    [Description("شادی")]
    [Route("/record_emotion_reply Joy")]
    Joy = 2,

    [Description("وجد")]
    [Route("/record_emotion_reply Ecstacy")]
    Ecstacy = 3,

    [Description("عشق")]
    [Route("/record_emotion_reply Love")]
    Love = 4,

    [Description("پذیرش")]
    [Route("/record_emotion_reply Acceptance")]
    Acceptance = 5,

    [Description("اعتماد")]
    [Route("/record_emotion_reply Trust")]
    Trust = 6,

    [Description("تحسین")]
    [Route("/record_emotion_reply Admiration")]
    Admiration = 7,

    [Description("سلطه‌پذیری")]
    [Route("/record_emotion_reply Submission")]
    Submission = 8,

    [Description("دلهره")]
    [Route("/record_emotion_reply Apprehension")]
    Apprehension = 9,

    [Description("ترس")]
    [Route("/record_emotion_reply Fear")]
    Fear = 10,

    [Description("وحشت")]
    [Route("/record_emotion_reply Terror")]
    Terror = 11,

    [Description("حیرت")]
    [Route("/record_emotion_reply Awe")]
    Awe = 12,

    [Description("حواس‌پرتی")]
    [Route("/record_emotion_reply Distraction")]
    Distraction = 13,

    [Description("تعجب")]
    [Route("/record_emotion_reply Surprise")]
    Surprise = 14,

    [Description("شگفتی")]
    [Route("/record_emotion_reply Amazement")]
    Amazement = 15,

    [Description("مخالفت")]
    [Route("/record_emotion_reply Disapproval")]
    Disapproval = 16,

    [Description("پکری")]
    [Route("/record_emotion_reply Pensiveness")]
    Pensiveness = 17,

    [Description("ناراحتی")]
    [Route("/record_emotion_reply Sadness")]
    Sadness = 18,

    [Description("سوگ")]
    [Route("/record_emotion_reply Grief")]
    Grief = 19,

    [Description("پشیمانی")]
    [Route("/record_emotion_reply Remorse")]
    Remorse = 20,

    [Description("ملال")]
    [Route("/record_emotion_reply Boredom")]
    Boredom = 21,

    [Description("بیزاری")]
    [Route("/record_emotion_reply Disgust")]
    Disgust = 22,

    [Description("انزجار")]
    [Route("/record_emotion_reply Loathing")]
    Loathing = 23,

    [Description("تحقیر")]
    [Route("/record_emotion_reply Contempt")]
    Contempt = 24,

    [Description("رنج")]
    [Route("/record_emotion_reply Annoyance")]
    Annoyance = 25,

    [Description("خشم")]
    [Route("/record_emotion_reply Anger")]
    Anger = 26,

    [Description("جنون")]
    [Route("/record_emotion_reply Rage")]
    Rage = 27,

    [Description("پرخاش‌گری")]
    [Route("/record_emotion_reply Aggressiveness")]
    Aggressiveness = 28,

    [Description("دل‌بستگی")]
    [Route("/record_emotion_reply Interest")]
    Interest = 29,

    [Description("انتظار")]
    [Route("/record_emotion_reply Anticipation")]
    Anticipation = 30,

    [Description("هشیاری")]
    [Route("/record_emotion_reply Vigilance")]
    Vigilance = 31,

    [Description("خوش‌بینی")]
    [Route("/record_emotion_reply Optimism")]
    Optimism = 32
}
