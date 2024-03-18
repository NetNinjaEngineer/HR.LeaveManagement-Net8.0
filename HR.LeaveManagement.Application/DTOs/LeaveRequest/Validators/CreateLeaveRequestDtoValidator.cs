using FluentValidation;

namespace HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators
{
    public class CreateLeaveRequestDtoValidator : AbstractValidator<CreateLeaveRequestDto>
    {
        public CreateLeaveRequestDtoValidator()
        {
            Include(new ILeaveRequestDtoValidator());
        }
    }
}
