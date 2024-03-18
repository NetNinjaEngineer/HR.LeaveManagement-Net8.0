using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Responses;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands
{
    public class UpdateLeaveTypeCommand : IRequest<UpdateCommandResponse>
    {
        public int Id { get; set; }
        public UpdateLeaveTypeDto LeaveTypeDto { get; set; } = null!;
    }
}
