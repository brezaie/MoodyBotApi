using Halood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halood.Repository.EntityFramework.Constraints;

public class UserEmotionConstraint : IEntityTypeConfiguration<UserEmotion>
{
    public void Configure(EntityTypeBuilder<UserEmotion> builder)
    {
        builder.ToTable("UserEmotions");
        builder.Property(x => x.EmotionText).HasMaxLength(100);
    }
}
