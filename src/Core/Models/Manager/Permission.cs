using Core.Models.Enums;
using System;

namespace Core.Models.Manager
{
    public class Permission
    {
        public long Id { get; set; }

        public PermissionType PermissionType { get; set; }

        public DateTime CreatedOn { get; set; }

        public string RoleId { get; set; }

        public Role Role { get; set; }
    }
}
