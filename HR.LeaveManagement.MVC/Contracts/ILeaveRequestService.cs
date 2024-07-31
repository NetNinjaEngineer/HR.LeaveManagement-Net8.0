using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts;

public interface ILeaveRequestService
{
    Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest);
    Task DeleteLeaveRequest(int id);
}
