using Halood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Halood.Repository.EntityFramework.Constraints;

public class UserConstraint: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(x => x.LanguageCode).HasMaxLength(5);
        builder.Property(x => x.FirstName).HasMaxLength(200);
        builder.Property(x => x.LastName).HasMaxLength(200);
        builder.Property(x => x.Username).HasMaxLength(200);
        builder.Property(x => x.IsGlobalSatisfactionReminderActive).HasDefaultValue(true);
    }
}
