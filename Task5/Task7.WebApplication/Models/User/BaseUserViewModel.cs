﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Task7.WebApplication.Models.User
{
    public class BaseUserViewModel
    {
        [Required(ErrorMessage = "Required field")]
        [StringLength(50, MinimumLength = 5)]
        [DisplayName("Username")]
        public string UserName { get; set; }
    }
}