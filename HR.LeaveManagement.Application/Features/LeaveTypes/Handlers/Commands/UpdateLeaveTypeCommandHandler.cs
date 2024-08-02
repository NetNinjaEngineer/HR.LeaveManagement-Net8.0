using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, UpdateCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeaveTypeCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateCommandResponse> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var updateCommandResponse = new UpdateCommandResponse();

            var leaveType = await _unitOfWork.Repository<LeaveType>()!.Get(request.Id);

            if (leaveType == null)
            {
                updateCommandResponse.StatusCode = 404;
                updateCommandResponse.Succeeded = false;
                updateCommandResponse.Message = $"LeaveTyoe with id ({request.Id}) does not exists.";
                return updateCommandResponse;
            }

            var updateLeaveTypeValidator = new UpdateLeaveTypeDtoValidator();
            var validationResults = await updateLeaveTypeValidator.ValidateAsync(request.LeaveTypeDto, cancellationToken);

            if (!validationResults.IsValid)
            {
                // throw new ValidationException(validationResults);
                updateCommandResponse.Succeeded = false;
                updateCommandResponse.Message = "Update Failed";
                updateCommandResponse.Errors = validationResults.Errors.Select(e => e.ErrorMessage).ToList();
                return updateCommandResponse;
            }

            _mapper.Map(request.LeaveTypeDto, leaveType);
            await _unitOfWork.Repository<LeaveType>()!.Update(leaveType);
            await _unitOfWork.SaveAsync();

            updateCommandResponse.StatusCode = 204;
            updateCommandResponse.Succeeded = true;
            updateCommandResponse.Message = "Updated Completed";

            return updateCommandResponse;
        }
    }
}
