using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Responses;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands
{
    public class CreateLeaveRequestCommand : IRequest<CreateCommandResponse>
    {
        public CreateLeaveRequestDto CreateLeaveRequestDto { get; set; } = null!;
    }
}
