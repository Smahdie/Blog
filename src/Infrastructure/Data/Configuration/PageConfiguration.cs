using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.Property(a => a.Language).IsRequired().HasMaxLength(8);
            builder.Property(a => a.Keyword).IsRequired().HasMaxLength(32);
            builder.HasIndex(a => new { a.Language, a.Keyword }).IsUnique();
        }
    }
}
