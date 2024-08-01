using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationDetailRequestHandler : IRequestHandler<GetLeaveAllocationDetailRequest, LeaveAllocationDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IEmployeeService _employeeService;

        public GetLeaveAllocationDetailRequestHandler(IMapper mapper, ILeaveAllocationRepository leaveAllocationRepository, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _employeeService = employeeService;
        }

        public async Task<LeaveAllocationDto> Handle(GetLeaveAllocationDetailRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocation = _mapper.Map<LeaveAllocationDto>(await _leaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id));
            leaveAllocation.Employee = await _employeeService.GetEmployeeById(leaveAllocation.RequestingEmployeeId);
            return leaveAllocation;
        }
    }
}
