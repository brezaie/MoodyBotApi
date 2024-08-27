using Halood.Domain.Enums;

namespace Halood.Domain.Dtos;

public class SatisfactionDistribution
{
    public SatisfactionLevel Satisfaction { get; set; }
    public int SatisfactionNumber { get; set; }
    public string SatisfactionName { get; set; }
    public int SatisfactionCount { get; set; }
    public float SatisfactionPercentage { get; set; }
    public byte[] SatisfactionBinaryColor { get; set; }
    public string SatisfactionColor { get; set; }
}
