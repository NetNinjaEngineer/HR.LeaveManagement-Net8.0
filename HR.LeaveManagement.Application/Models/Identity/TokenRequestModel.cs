using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Application.Models.Identity
{
    public class TokenRequestModel
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
