using Authentication_Authorization.Models.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Authentication_Authorization.Models.DTOs
{
    public class AssignedRoleDTO
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> HasRole { get; set; }
        public IEnumerable<AppUser> HasNotRole { get; set; }
        public string RoleName { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }
    }
}
