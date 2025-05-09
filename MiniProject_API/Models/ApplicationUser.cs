﻿using Microsoft.AspNetCore.Identity;

namespace MiniProject_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }   
    }
}
