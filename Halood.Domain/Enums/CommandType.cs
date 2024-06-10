using System.ComponentModel;

namespace Halood.Domain.Enums;

public enum CommandType
{
    Unknown = 0,

    [Description("/start")]
    Start = 1,

    [Description("/how_is_your_satisfaction")]
    Satisfaction = 2,

    [Description("/how_do_you_feel")]
    Feeling = 3
}
