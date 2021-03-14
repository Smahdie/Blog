using Core.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Core.Models.Manager
{
    public class Role : IdentityRole<string>, IAuditable
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString().ToLower();
        }

        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ICollection<ManagerRole> ManagerRoles { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
