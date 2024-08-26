using Halood.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Halood.Repository.EntityFramework.Constraints;

public class UserThoughtConstraint : IEntityTypeConfiguration<UserThought>
{
    public void Configure(EntityTypeBuilder<UserThought> builder)
    {
        builder.ToTable("UserThoughts");
        builder.Property(x => x.Text).HasMaxLength(4100);
    }
}
