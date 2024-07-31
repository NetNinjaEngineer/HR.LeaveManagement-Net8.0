using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HR.LeaveManagement.MVC.Models;

public class CreateLeaveRequestVM
{
    [Display(Name = "Start Date")]
    [Required]
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date")]
    [Required]
    public DateTime EndDate { get; set; }

    public IEnumerable<SelectListItem> LeaveTypes { get; set; } = [];

    [Display(Name = "Comments")]
    [MaxLength(300)]
    public string? RequestComments { get; set; }

    [Display(Name = "Leave Type")]
    public int LeaveTypeId { get; set; }
}
