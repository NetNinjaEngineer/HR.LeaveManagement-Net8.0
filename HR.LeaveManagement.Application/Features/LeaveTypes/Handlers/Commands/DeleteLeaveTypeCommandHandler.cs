using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, DeleteCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public DeleteLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<DeleteCommandResponse> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var deleteCommandResponse = new DeleteCommandResponse();
            var leaveType = await _leaveTypeRepository.Get(request.Id);
            if (leaveType == null)
            {
                deleteCommandResponse.StatusCode = 404;
                deleteCommandResponse.Message = $"Leave type with id ({request.Id}) was not found!.";
                deleteCommandResponse.Succeeded = false;
                return deleteCommandResponse;
            }

            await _leaveTypeRepository.Delete(leaveType);

            deleteCommandResponse.Succeeded = true;
            deleteCommandResponse.Message = "Delete Successfully";
            deleteCommandResponse.StatusCode = 204;

            return deleteCommandResponse;
        }
    }
}
