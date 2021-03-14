using Core.Models.Enums;
using Core.Models.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Configuration
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasOne(a => a.Role).WithMany(a => a.Permissions).HasForeignKey(a => a.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(a => a.PermissionType).HasConversion(new EnumToStringConverter<PermissionType>());
        }
    }
}
