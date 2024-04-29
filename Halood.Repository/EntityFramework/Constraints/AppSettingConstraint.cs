using Halood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halood.Repository.EntityFramework.Constraints;

public class AppSettingConstraint : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.ToTable("AppSettings");
        builder.Property(x => x.DefaultSatisfactionScheduler).HasDefaultValue(new TimeSpan(22, 0, 0));
    }
}
