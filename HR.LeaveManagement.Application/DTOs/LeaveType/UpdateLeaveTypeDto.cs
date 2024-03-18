namespace HR.LeaveManagement.Application.DTOs.LeaveType
{
    public class UpdateLeaveTypeDto : ILeaveTypeDto
    {
        public string? Name { get; set; }
        public int DefaultDays { get; set; }
    }
}
