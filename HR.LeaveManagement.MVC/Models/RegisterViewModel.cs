using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.MVC.Models
{
	public class RegisterViewModel
	{
		[Required]
		public string? FirstName { get; set; }

		[Required]
		public string? LastName { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset DateOfBirth { get; set; }

		[Required]
		public string? UserName { get; set; }

		[Required]
		[EmailAddress]
		public string? Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string? Password { get; set; }
	}
}
