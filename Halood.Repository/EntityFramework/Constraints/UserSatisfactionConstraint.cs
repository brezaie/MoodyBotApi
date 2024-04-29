using Halood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halood.Repository.EntityFramework.Constraints;

public class UserSatisfactionConstraint : IEntityTypeConfiguration<UserSatisfaction>
{
    public void Configure(EntityTypeBuilder<UserSatisfaction> builder)
    {
        builder.ToTable("UserSatisfactions");
    }
}
