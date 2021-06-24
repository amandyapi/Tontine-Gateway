using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TontineGateway.Models
{
    public class User
    {
        public string Token { get; set; }
        public bool IsSkSuperAdmin { get; set; }
        public string Id { get; set; }
        public string SecurityStamp { get; set; }
        public Role[] Roles { get; set; }
        public Permissionset[] PermissionSets { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ClientId { get; set; }
        public int Language { get; set; }
    }

    public class Role
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string ClientId { get; set; }
    }

    public class Permissionset
    {
        public string ResourceId { get; set; }
        public int ClaimValue { get; set; }
        public string ClaimValueDescription { get; set; }
        public string ClientId { get; set; }
    }
}




