using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class CreateLeaveRequestCommandHandler
        : IRequestHandler<CreateLeaveRequestCommand, CreateCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateLeaveRequestCommandHandler(
            IMapper mapper,
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IEmailSender emailSender,
            IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
        }

        public async Task<CreateCommandResponse> Handle(
            CreateLeaveRequestCommand request,
            CancellationToken cancellationToken)
        {
            var createCommandResponse = new CreateCommandResponse();
            var createLeaveTypeValidator = new CreateLeaveRequestDtoValidator();
            var validationResults = await createLeaveTypeValidator.ValidateAsync(request.CreateLeaveRequestDto);

            var userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(
                q => q.Type == CustomClaimTypes.Uid)?.Value;

            if (!validationResults.IsValid)
            {
                createCommandResponse.Succeeded = false;
                createCommandResponse.Message = "Creation Faild";
                createCommandResponse.Errors = validationResults.Errors.Select(e => e.ErrorMessage).ToList();
                return createCommandResponse;
            }
            else
            {

                var leaveTypeIdValid = await _leaveTypeRepository.Exists(request.CreateLeaveRequestDto.LeaveTypeId);
                if (!leaveTypeIdValid)
                {
                    createCommandResponse.Succeeded = false;
                    createCommandResponse.Message = $"There is no leaveType with id ({request.CreateLeaveRequestDto.LeaveTypeId})";
                    return createCommandResponse;
                }

                LeaveRequest? leaveRequest = _mapper.Map<LeaveRequest>(request.CreateLeaveRequestDto);
                leaveRequest.RequestingEmployeeId = userId;
                leaveRequest = await _leaveRequestRepository.Add(leaveRequest);

                var emailAddress = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

                var email = new Email()
                {
                    To = emailAddress,
                    Body = $"Your leaveRequest for {request.CreateLeaveRequestDto.StartDate} to {request.CreateLeaveRequestDto.EndDate} " +
                    $"has been summited successfully",
                    Subject = "Leave Request Summited"
                };

                //try
                //{
                //    await _emailSender.SendEmail(email);
                //}
                //catch (Exception ex)
                //{

                //}

                createCommandResponse.Message = "Created Successfully";
                createCommandResponse.Succeeded = true;
                createCommandResponse.Id = leaveRequest.Id;
                return createCommandResponse;

            }
        }
    }
}
