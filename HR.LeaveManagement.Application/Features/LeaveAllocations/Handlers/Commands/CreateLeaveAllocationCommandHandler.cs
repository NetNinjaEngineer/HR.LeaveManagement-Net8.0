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
        private readonly IEmployeeService _employeeService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveAllocationCommandHandler(
            IMapper mapper,
            IEmployeeService employeeService,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var createLeaveAllocationValidator = new CreateLeaveAllocationDtoValidator(_unitOfWork.LeaveTypeRepository);
            var validationResults = await createLeaveAllocationValidator.ValidateAsync(request.CreateLeaveAllocationDto);
            if (!validationResults.IsValid)
                throw new ValidationException(validationResults);

            var leaveType = await _unitOfWork.Repository<LeaveType>()!.Get(request.CreateLeaveAllocationDto.LeaveTypeId);
            var employees = await _employeeService.GetEmployeesAsync();
            var period = DateTime.Now.Year;
            var allocations = new List<LeaveAllocation>();

            foreach (var employee in employees)
            {
                if (await _unitOfWork.LeaveAllocationRepository.AllocationsExists(employee.EmployeeId, leaveType.Id, period))
                    continue;

                allocations.Add(new LeaveAllocation
                {
                    EmployeeId = employee.EmployeeId,
                    LeaveTypeId = leaveType.Id,
                    NumberOfDays = leaveType.DefaultDays,
                    Period = period
                });

            }

            await _unitOfWork.LeaveAllocationRepository.AddAllocations(allocations);
            response.Succeeded = true;
            response.Message = "Allocations Successful";

            return response;
        }
    }
}
