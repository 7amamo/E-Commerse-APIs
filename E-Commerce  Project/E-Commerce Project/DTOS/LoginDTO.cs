﻿using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Project.DTOS
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }


    }
}
