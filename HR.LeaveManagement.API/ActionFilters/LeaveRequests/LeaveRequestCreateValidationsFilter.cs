using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HR.LeaveManagement.API;

public class LeaveRequestCreateValidationsFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var createLeaveRequestDto = (CreateLeaveRequestDto)context.ActionArguments["leaveRequest"]!;
        var createLeaveRequestDtoValidator = new CreateLeaveRequestDtoValidator();
        var validationResults = await createLeaveRequestDtoValidator.ValidateAsync(createLeaveRequestDto);
        if (!validationResults.IsValid)
        {
            var modelErrors = validationResults.Errors.Select(e => e.ErrorMessage).ToList();
            context.Result = new UnprocessableEntityObjectResult(modelErrors);
            return;
        }

        await next();

    }
}
