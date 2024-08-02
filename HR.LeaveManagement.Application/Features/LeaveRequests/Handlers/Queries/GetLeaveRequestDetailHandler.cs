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
        private readonly IEmployeeService _employeeService;
        private readonly IUnitOfWork _unitOfWork;

        public GetLeaveRequestDetailHandler(
            IMapper mapper,
            IEmployeeService employeeService,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LeaveRequestDto> Handle(
            GetLeaveRequestDetail request,
            CancellationToken cancellationToken)
        {
            var leaveRequest = _mapper.Map<LeaveRequestDto>(await _unitOfWork.LeaveRequestRepository.GetLeaveRequestWithDetails(request.Id));
            leaveRequest.Employee = await _employeeService.GetEmployeeById(leaveRequest.RequestingEmployeeId);
            return leaveRequest;
        }
    }
}
