using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(a => a.Name).IsRequired().HasMaxLength(64);
            builder.Property(a => a.Language).IsRequired().HasMaxLength(8);
            builder.HasIndex(a => new { a.Name, a.Language }).IsUnique();
        }
    }
}
