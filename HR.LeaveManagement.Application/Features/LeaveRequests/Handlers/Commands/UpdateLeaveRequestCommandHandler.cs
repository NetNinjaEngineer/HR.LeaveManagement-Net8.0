using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public UpdateLeaveRequestCommandHandler(IMapper mapper, ILeaveRequestRepository leaveRequestRepository, ILeaveAllocationRepository leaveAllocationRepository)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.Get(request.Id);
            if (leaveRequest is null)
                throw new NotFoundException(nameof(leaveRequest), leaveRequest!.Id);

            if (request.UpdateLeaveRequestDto != null)
            {
                var updateLeaveRequestValidator = new UpdateLeaveRequestDtoValidator();
                var validationResults = await updateLeaveRequestValidator.ValidateAsync(request.UpdateLeaveRequestDto);
                if (!validationResults.IsValid)
                    throw new ValidationException(validationResults);

                _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);
                await _leaveRequestRepository.Update(leaveRequest);

            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                await _leaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
                if (request.ChangeLeaveRequestApprovalDto.Approved.Value)
                {
                    var allocation = await _leaveAllocationRepository.GetUserAllocations(userId: leaveRequest.RequestingEmployeeId, leaveTypeId: leaveRequest.LeaveTypeId);
                    if (allocation != null)
                    {
                        int daysRequested = (int)(request.UpdateLeaveRequestDto.EndDate - request.UpdateLeaveRequestDto.StartDate).TotalDays;
                        allocation.NumberOfDays -= daysRequested;

                        await _leaveAllocationRepository.Update(allocation);
                    }
                }
            }

            return Unit.Value;

        }
    }
}
