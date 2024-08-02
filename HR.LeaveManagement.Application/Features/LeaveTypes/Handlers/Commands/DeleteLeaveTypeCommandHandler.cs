using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, DeleteCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveTypeCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteCommandResponse> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var deleteCommandResponse = new DeleteCommandResponse();
            var leaveType = await _unitOfWork.Repository<LeaveType>()!.Get(request.Id);
            if (leaveType == null)
            {
                deleteCommandResponse.StatusCode = 404;
                deleteCommandResponse.Message = $"Leave type with id ({request.Id}) was not found!.";
                deleteCommandResponse.Succeeded = false;
                return deleteCommandResponse;
            }

            await _unitOfWork.Repository<LeaveType>()!.Delete(leaveType);
            await _unitOfWork.SaveAsync();

            deleteCommandResponse.Id = request.Id;
            deleteCommandResponse.Succeeded = true;
            deleteCommandResponse.Message = "Delete Successfully";
            deleteCommandResponse.StatusCode = 204;

            return deleteCommandResponse;
        }
    }
}
