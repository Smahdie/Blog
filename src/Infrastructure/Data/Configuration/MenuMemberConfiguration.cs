using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class MenuMemberConfiguration : IEntityTypeConfiguration<MenuMember>
    {
        public void Configure(EntityTypeBuilder<MenuMember> builder)
        {
            builder.HasOne(a => a.Menu).WithMany(a=>a.Members).HasForeignKey(a => a.MenuId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
