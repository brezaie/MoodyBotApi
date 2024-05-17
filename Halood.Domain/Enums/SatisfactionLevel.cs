using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum SatisfactionLevel
{
    [Description("😥")]
    Awful = 1,

    [Description("😞")]
    Bad = 2,

    [Description("😏")]
    SoSo = 3,

    [Description("😊")]
    Good = 4,

    [Description("😍")]
    Perfect = 5,
}
