using Halood.Domain.Enums;

namespace Halood.Domain.Dtos;

public class EmotionDistribution
{
    public Emotion Emotion { get; set; }
    public string EmotionName { get; set; }
    public int EmotionCount { get; set; }
    public float EmotionPercentage { get; set; }
    public byte[] EmotionBinaryColor { get; set; }
    public string EmotionColor { get; set; }
}
