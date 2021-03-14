using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class Content2TagConfiguration : IEntityTypeConfiguration<Content2Tag>
    {
        public void Configure(EntityTypeBuilder<Content2Tag> builder)
        {
            builder.HasKey(a => new { a.ContentId, a.TagId});
            builder.HasOne(a => a.Tag).WithMany().HasForeignKey(a => a.TagId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.Content).WithMany(a=>a.Tags).HasForeignKey(a => a.ContentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
