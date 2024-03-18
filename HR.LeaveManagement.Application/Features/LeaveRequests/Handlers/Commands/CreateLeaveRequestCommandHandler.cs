using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, CreateCommandResponse>
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

        public async Task<CreateCommandResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var createCommandResponse = new CreateCommandResponse();
            var createLeaveTypeValidator = new CreateLeaveRequestDtoValidator();
            var validationResults = await createLeaveTypeValidator.ValidateAsync(request.CreateLeaveRequestDto);
            if (!validationResults.IsValid)
            {
                createCommandResponse.Succeeded = false;
                createCommandResponse.Message = "Creation Faild";
                createCommandResponse.Errors = validationResults.Errors.Select(e => e.ErrorMessage).ToList();
                return createCommandResponse;
            }

            var leaveTypeIdValid = await _leaveTypeRepository.Exists(request.CreateLeaveRequestDto.LeaveTypeId);
            if (!leaveTypeIdValid)
            {
                createCommandResponse.Succeeded = false;
                createCommandResponse.Message = $"There is no leaveType with id ({request.CreateLeaveRequestDto.LeaveTypeId})";
                return createCommandResponse;
            }

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

            createCommandResponse.Message = "Created Successfully";
            createCommandResponse.Succeeded = true;
            return createCommandResponse;
        }
    }
}
