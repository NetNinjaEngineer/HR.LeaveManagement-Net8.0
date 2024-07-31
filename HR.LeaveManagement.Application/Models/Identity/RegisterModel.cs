using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.Application.Models.Identity
{
    public class RegisterModel
    {
        [Required, StringLength(100)]
        public string? FirstName { get; set; }

        [Required, StringLength(100)]
        public string? LastName { get; set; }

        [Required, StringLength(50)]
        public string? Username { get; set; }

        [Required, StringLength(128)]
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(265)]
        public string? Password { get; set; }
    }
}
