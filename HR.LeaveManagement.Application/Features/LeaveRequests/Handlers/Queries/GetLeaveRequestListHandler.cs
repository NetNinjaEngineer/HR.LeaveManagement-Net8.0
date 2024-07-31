using AutoMapper;
using HR.LeaveManagement.Application.Constants;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestListHandler : IRequestHandler<GetLeaveRequestList, List<LeaveRequestListDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetLeaveRequestListHandler(IMapper mapper, ILeaveRequestRepository leaveRequestRepository, IEmployeeService employeeService, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
            _employeeService = employeeService;
            _contextAccessor = contextAccessor;
        }

        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestList request, CancellationToken cancellationToken)
        {
            var leaveRequests = new List<LeaveRequest>();
            var requests = new List<LeaveRequestListDto>();

            if (request.IsLoggedInUser)
            {
                var userId = _contextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;
                leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails(userId!);
                var employee = await _employeeService.GetEmployeeById(userId);
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
                foreach (var req in requests)
                {
                    req.Employee = employee;
                }
            }

            else
            {
                leaveRequests = await _leaveRequestRepository.GetLeaveRequestsWithDetails();
                requests = _mapper.Map<List<LeaveRequestListDto>>(leaveRequests);
                foreach (var req in requests)
                {
                    req.Employee = await _employeeService.GetEmployeeById(req.RequestingEmployeeId);
                }
            }


            return requests;
        }
    }
}
