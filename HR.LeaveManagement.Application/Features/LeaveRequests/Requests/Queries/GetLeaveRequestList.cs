using HR.LeaveManagement.Application.Dtos.LeaveRequest;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries
{
    public class GetLeaveRequestList : IRequest<List<LeaveRequestListDto>>
    {
    }
}
