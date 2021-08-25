using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class Content2CategoryConfiguration : IEntityTypeConfiguration<Content2Category>
    {
        public void Configure(EntityTypeBuilder<Content2Category> builder)
        {
            builder.HasKey(a => new { a.ContentId, a.CategoryId });
            builder.HasOne(a => a.Category).WithMany().HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Content).WithMany(a => a.Categories).HasForeignKey(a => a.ContentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
