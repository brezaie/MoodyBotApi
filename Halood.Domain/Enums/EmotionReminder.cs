using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum EmotionReminder
{
    Unknown = 0,
    [Description("Seven")]
    Seven = 7,

    [Description("Nine")]
    Nine = 9,

    [Description("Eleven")]
    Eleven = 11,

    [Description("Thirteen")]
    Thirteen = 13,

    [Description("Fifteen")]
    Fifteen = 15,

    [Description("Seventeen")]
    Seventeen = 17,

    [Description("Nineteen")]
    Nineteen = 19,

    [Description("TwentyOne")]
    TwentyOne = 21,

    [Description("TwentyThree")]
    TwentyThree = 23,

    [Description("Submit")]
    Submit = 25
}
