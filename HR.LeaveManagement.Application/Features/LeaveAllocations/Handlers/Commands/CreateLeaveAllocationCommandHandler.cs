﻿using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IEmployeeService _employeeService;

        public CreateLeaveAllocationCommandHandler(IMapper mapper, ILeaveAllocationRepository leaveAllocationRepository, ILeaveTypeRepository leaveTypeRepository, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _employeeService = employeeService;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var createLeaveAllocationValidator = new CreateLeaveAllocationDtoValidator(_leaveTypeRepository);
            var validationResults = await createLeaveAllocationValidator.ValidateAsync(request.CreateLeaveAllocationDto);
            if (!validationResults.IsValid)
                throw new ValidationException(validationResults);

            var leaveType = await _leaveTypeRepository.Get(request.CreateLeaveAllocationDto.LeaveTypeId);
            var employees = await _employeeService.GetEmployeesAsync();
            var period = DateTime.Now.Year;
            var allocations = new List<LeaveAllocation>();

            foreach (var employee in employees)
            {
                if (await _leaveAllocationRepository.AllocationsExists(employee.EmployeeId, leaveType.Id, period))
                    continue;

                allocations.Add(new LeaveAllocation
                {
                    EmployeeId = employee.EmployeeId,
                    LeaveTypeId = leaveType.Id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period
                });

            }

            await _leaveAllocationRepository.AddAllocations(allocations);
            response.Succeeded = true;
            response.Message = "Allocations Successful";

            return response;
        }
    }
}
