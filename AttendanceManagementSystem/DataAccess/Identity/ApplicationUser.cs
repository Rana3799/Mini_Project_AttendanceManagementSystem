﻿using AttendanceManagementSystem.DataAccess.Entities.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementSystem.DataAccess.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom fields if needed
        [Key]
        public int userID { get; set; } 
        public string? FullName { get; set; }
        public string? OrganizationId { get; set; }
        public Organization? Organization { get; set; }

    }
}
