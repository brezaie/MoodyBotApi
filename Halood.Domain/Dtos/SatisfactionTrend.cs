using Halood.Domain.Enums;

namespace Halood.Domain.Dtos;

public class SatisfactionTrend
{
    public SatisfactionLevel Satisfaction { get; set; }
    public float SatisfactionLevel { get; set; }
    public string Date { get; set; }

}
