using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum SatisfactionLevel
{
    [Description("Awful")]
    Awful = 1,
    
    [Description("Bad")]
    Bad = 2,

    [Description("SoSo")]
    SoSo = 3,

    [Description("Good")]
    Good = 4,

    [Description("Percect")]
    Perfect = 5,
}
