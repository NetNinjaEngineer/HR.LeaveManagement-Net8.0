using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class CreateLeaveTypeRequestHandler : IRequestHandler<CreateLeaveTypeCommand, CreateCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveTypeRequestHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCommandResponse> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var createCommandResponse = new CreateCommandResponse();
            var validator = new CreateLeaveTypeDtoValidator();

            var validationResult = await validator.ValidateAsync(request.CreateLeaveTypeDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                createCommandResponse.Succeeded = false;
                createCommandResponse.Message = "Created Faild";
                createCommandResponse.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return createCommandResponse;
            }

            var mappedLeaveType = _mapper.Map<CreateLeaveTypeDto, LeaveType>(request.CreateLeaveTypeDto);

            mappedLeaveType = await _unitOfWork.Repository<LeaveType>()!.Add(mappedLeaveType);
            await _unitOfWork.SaveAsync();

            createCommandResponse.Message = $"Created Successfully with id ({mappedLeaveType.Id})";
            createCommandResponse.Id = mappedLeaveType.Id;

            return createCommandResponse;
        }
    }
}
