using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries
{
    public class GetLeaveRequestList : IRequest<List<LeaveRequestListDto>>
    {
        public bool IsLoggedInUser { get; set; }
    }
}
