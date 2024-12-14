﻿using Microsoft.AspNetCore.Identity;

namespace E_commerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
