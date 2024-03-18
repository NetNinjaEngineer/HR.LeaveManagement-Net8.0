using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IEmailSender _emailSender;

        public CreateLeaveRequestCommandHandler(IMapper mapper, ILeaveRequestRepository leaveRequestRepository, ILeaveTypeRepository leaveTypeRepository, IEmailSender emailSender)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _emailSender = emailSender;
        }

        public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var createLeaveTypeValidator = new CreateLeaveRequestDtoValidator(_leaveTypeRepository);
            var validationResults = await createLeaveTypeValidator.ValidateAsync(request.CreateLeaveRequestDto);
            if (!validationResults.IsValid)
                throw new ValidationException(validationResults);

            LeaveRequest? leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDto);
            leaveRequest = await _leaveRequestRepository.Add(leaveRequest);

            var email = new Email()
            {
                To = "employee@org.com",
                Body = $"Your leaveRequest for {request.CreateLeaveRequestDto.StartDate} to {request.CreateLeaveRequestDto.EndDate} " +
                $"has been summited successfully",
                Subject = "Leave Request Summited"
            };

            try
            {
                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {

            }

            return leaveRequest.Id;
        }
    }
}
