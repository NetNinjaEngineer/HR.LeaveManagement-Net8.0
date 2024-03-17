using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Dtos.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Queries
{
    public class GetLeaveRequestListHandler : IRequestHandler<GetLeaveRequestList, List<LeaveRequestListDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public GetLeaveRequestListHandler(IMapper mapper, ILeaveRequestRepository leaveRequestRepository)
        {
            _mapper = mapper;
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<List<LeaveRequestListDto>> Handle(GetLeaveRequestList request, CancellationToken cancellationToken)
        {
            var leaveRequests = await _leaveRequestRepository.GetAll();
            return _mapper.Map<IReadOnlyList<LeaveRequest>, List<LeaveRequestListDto>>(leaveRequests);
        }
    }
}
