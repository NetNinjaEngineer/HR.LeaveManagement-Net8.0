using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Requests.Commands;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public UpdateLeaveAllocationCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var updateLeaveAllocationValidator = new UpdateLeaveAllocationDtoValidator(unitOfWork.LeaveTypeRepository);
            var validationResults = await updateLeaveAllocationValidator.ValidateAsync(request.UpdateLeaveAllocationDto);
            if (!validationResults.IsValid)
                throw new Exception();

            LeaveAllocation? leaveAllocation = await unitOfWork.Repository<LeaveAllocation>()!.Get(request.UpdateLeaveAllocationDto.Id);
            mapper.Map(request.UpdateLeaveAllocationDto, leaveAllocation);
            await unitOfWork.Repository<LeaveAllocation>()!.Update(leaveAllocation);
            return Unit.Value;
        }
    }
}
