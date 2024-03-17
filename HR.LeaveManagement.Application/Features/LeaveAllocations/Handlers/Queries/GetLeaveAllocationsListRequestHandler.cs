using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Dtos;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Queries;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Queries
{
    public class GetLeaveAllocationsListRequestHandler : IRequestHandler<GetLeaveAllocationsListRequest, List<LeaveAllocationDto>>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;

        public GetLeaveAllocationsListRequestHandler(IMapper mapper, ILeaveAllocationRepository leaveAllocationRepository)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
        }

        public async Task<List<LeaveAllocationDto>> Handle(GetLeaveAllocationsListRequest request, CancellationToken cancellationToken)
        {
            IReadOnlyList<LeaveAllocation>? leaveAllocations = await _leaveAllocationRepository.GetAll();
            return _mapper.Map<IReadOnlyList<LeaveAllocation>, List<LeaveAllocationDto>>(leaveAllocations);
        }
    }
}
