using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public UpdateLeaveRequestCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.Repository<LeaveRequest>()!.Get(request.Id);
            if (leaveRequest is null)
                throw new NotFoundException(nameof(leaveRequest), leaveRequest!.Id);

            if (request.UpdateLeaveRequestDto != null)
            {
                var updateLeaveRequestValidator = new UpdateLeaveRequestDtoValidator();
                var validationResults = await updateLeaveRequestValidator.ValidateAsync(request.UpdateLeaveRequestDto, cancellationToken);
                if (!validationResults.IsValid)
                    throw new ValidationException(validationResults);

                _mapper.Map(request.UpdateLeaveRequestDto, leaveRequest);
                await _unitOfWork.Repository<LeaveRequest>()!.Update(leaveRequest);
            }
            else if (request.ChangeLeaveRequestApprovalDto != null)
            {
                await _unitOfWork.LeaveRequestRepository.ChangeApprovalStatus(leaveRequest, request.ChangeLeaveRequestApprovalDto.Approved);
                if (request.ChangeLeaveRequestApprovalDto.Approved.Value)
                {
                    var allocation = await _unitOfWork.LeaveAllocationRepository.GetUserAllocations(userId: leaveRequest.RequestingEmployeeId, leaveTypeId: leaveRequest.LeaveTypeId);
                    if (allocation != null)
                    {
                        int daysRequested = (int)(request.UpdateLeaveRequestDto.EndDate - request.UpdateLeaveRequestDto.StartDate).TotalDays;
                        allocation.NumberOfDays -= daysRequested;

                        await _unitOfWork.Repository<LeaveAllocation>()!.Update(allocation);
                    }
                }
            }

            await _unitOfWork.SaveAsync();

            return Unit.Value;

        }
    }
}
