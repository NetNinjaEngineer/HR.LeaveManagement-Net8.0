using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestDetailHandler : IRequestHandler<GetLeaveRequestDetail, LeaveRequestDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeService _employeeService;

        public GetLeaveRequestDetailHandler(
            IMapper mapper,
            ILeaveRequestRepository leaveRequestRepository,
            IEmployeeService employeeService)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _employeeService = employeeService;
        }

        public async Task<LeaveRequestDto> Handle(
            GetLeaveRequestDetail request,
            CancellationToken cancellationToken)
        {
            var leaveRequest = _mapper.Map<LeaveRequestDto>(await _leaveRequestRepository.GetLeaveRequestWithDetails(request.Id));
            leaveRequest.Employee = await _employeeService.GetEmployeeById(leaveRequest.RequestingEmployeeId);
            return leaveRequest;
        }
    }
}
