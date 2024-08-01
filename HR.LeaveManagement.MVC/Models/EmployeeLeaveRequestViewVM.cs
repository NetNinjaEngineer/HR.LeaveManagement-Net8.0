namespace HR.LeaveManagement.MVC.Models;

public class EmployeeLeaveRequestViewVM
{
    public List<LeaveAllocationVM> LeaveAllocations { get; set; } = [];
    public List<LeaveRequestVM> LeaveRequests { get; set; } = [];
}