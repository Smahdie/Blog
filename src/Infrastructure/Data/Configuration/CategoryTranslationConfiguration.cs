using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.HasOne(a => a.Category).WithMany(a => a.Translations).HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(a => a.Language).HasMaxLength(8);
        }
    }
}
