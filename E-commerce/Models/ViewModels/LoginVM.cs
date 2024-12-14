﻿using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models.ViewModels
{
    public class LoginVM
    {
        public int Id { get; set; }
        [Required]
        public string Account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RemeberMe { get; set; }
    }
}
