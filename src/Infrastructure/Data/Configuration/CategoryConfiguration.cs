using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasOne(a => a.Parent).WithMany(a => a.Children).HasForeignKey(a => a.ParentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
