using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum SatisfactionLevel
{
    [Description("خیلی کم 😥")]
    Awful = 1,
    
    [Description("کم 😞")]
    Bad = 2,

    [Description("متوسط 😏")]
    SoSo = 3,

    [Description("زیاد 😊")]
    Good = 4,

    [Description("خیلی زیاد 😍")]
    Perfect = 5,
}
