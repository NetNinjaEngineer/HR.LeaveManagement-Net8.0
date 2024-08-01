using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationsListRequestHandler : IRequestHandler<GetLeaveAllocationsListRequest, List<LeaveAllocationDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;

        public GetLeaveAllocationsListRequestHandler(IMapper mapper,
            ILeaveAllocationRepository leaveAllocationRepository,
            IHttpContextAccessor contextAccessor,
            IEmployeeService employeeService)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
        }

        public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationsListRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocations = new List<LeaveAllocation>();
            var allocations = new List<LeaveAllocationDto>();

            if (request.IsLoggedInUser)
            {
                var userId = _contextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;
                leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails(userId!);
                var employee = await _employeeService.GetEmployeeById(userId);
                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
                foreach (var allocation in allocations)
                {
                    allocation.Employee = employee;
                }
            }

            else
            {
                leaveAllocations = await _leaveAllocationRepository.GetLeaveAllocationsWithDetails();
                allocations = _mapper.Map<List<LeaveAllocationDto>>(leaveAllocations);
                foreach (var req in allocations)
                {
                    req.Employee = await _employeeService.GetEmployeeById(req.RequestingEmployeeId);
                }
            }


            return allocations;
        }
    }
}
