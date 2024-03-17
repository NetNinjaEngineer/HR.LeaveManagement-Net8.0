namespace HR.LeaveManagement.Application.DTOs.LeaveType
{
    internal interface ILeaveTypeDto
    {
        public string? Name { get; set; }

        public int DefaultDays { get; set; }
    }
}
