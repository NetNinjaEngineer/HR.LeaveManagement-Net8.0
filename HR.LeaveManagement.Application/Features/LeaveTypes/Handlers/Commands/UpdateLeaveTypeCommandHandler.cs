using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, UpdateCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public UpdateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<UpdateCommandResponse> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var updateCommandResponse = new UpdateCommandResponse();

            var leaveType = await _leaveTypeRepository.Get(request.Id);

            if (leaveType == null)
            {
                updateCommandResponse.StatusCode = 404;
                updateCommandResponse.Succeeded = false;
                updateCommandResponse.Message = $"LeaveTyoe with id ({request.Id}) does not exists.";
                return updateCommandResponse;
            }

            var updateLeaveTypeValidator = new UpdateLeaveTypeDtoValidator();
            var validationResults = await updateLeaveTypeValidator.ValidateAsync(request.LeaveTypeDto);

            if (!validationResults.IsValid)
            {
                // throw new ValidationException(validationResults);
                updateCommandResponse.Succeeded = false;
                updateCommandResponse.Message = "Update Failed";
                updateCommandResponse.Errors = validationResults.Errors.Select(e => e.ErrorMessage).ToList();
                return updateCommandResponse;
            }

            _mapper.Map(request.LeaveTypeDto, leaveType);
            await _leaveTypeRepository.Update(leaveType);

            updateCommandResponse.StatusCode = 204;
            updateCommandResponse.Succeeded = true;
            updateCommandResponse.Message = "Updated Completed";

            return updateCommandResponse;
        }
    }
}
