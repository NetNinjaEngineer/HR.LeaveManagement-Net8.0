using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HR.LeaveManagement.API.ActionFilters.LeaveType
{
    public class LeaveTypeCreateValidationsFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var leaveType = (CreateLeaveTypeDto)context.ActionArguments["createLeaveTypeDto"]!;
            var createLeaveTypeDtoValidator = new CreateLeaveTypeDtoValidator();
            var validationResults = createLeaveTypeDtoValidator.Validate(leaveType);
            if (!validationResults.IsValid)
                context.Result = new UnprocessableEntityResult();
        }
    }
}
