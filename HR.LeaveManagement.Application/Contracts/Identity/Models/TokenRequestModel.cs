﻿using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Application.Contracts.Identity.Models
{
    public class TokenRequestModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
