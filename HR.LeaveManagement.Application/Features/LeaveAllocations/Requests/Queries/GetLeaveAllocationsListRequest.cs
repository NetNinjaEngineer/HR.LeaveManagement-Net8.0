using HR.LeaveManagement.Application.Dtos;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries
{
    public class GetLeaveAllocationsListRequest : IRequest<List<LeaveAllocationDto>>
    {
    }
}
