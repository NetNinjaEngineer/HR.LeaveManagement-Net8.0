using HR.LeaveManagement.Application.Dtos.LeaveRequest;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands
{
    public class UpdateLeaveRequestCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public UpdateLeaveRequestDto UpdateLeaveRequestDto { get; set; } = null!;

        public ChangeLeaveRequestApprovalDto ChangeLeaveRequestApprovalDto { get; set; } = null!;
    }
}
