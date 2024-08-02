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
        private readonly IEmployeeService _employeeService;
        private readonly IUnitOfWork _unitOfWork;

        public GetLeaveAllocationDetailRequestHandler(IMapper mapper, IEmployeeService employeeService, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LeaveAllocationDto> Handle(GetLeaveAllocationDetailRequest request, CancellationToken cancellationToken)
        {
            var leaveAllocation = _mapper.Map<LeaveAllocationDto>(await _unitOfWork.LeaveAllocationRepository.GetLeaveAllocationWithDetails(request.Id));
            leaveAllocation.Employee = await _employeeService.GetEmployeeById(leaveAllocation.RequestingEmployeeId);
            return leaveAllocation;
        }
    }
}
