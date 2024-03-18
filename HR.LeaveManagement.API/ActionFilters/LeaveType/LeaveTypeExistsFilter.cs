using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HR.LeaveManagement.API.ActionFilters.LeaveType
{
    public class LeaveTypeExistsFilter : IActionFilter
    {
        private readonly IMediator _mediator;

        public LeaveTypeExistsFilter(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var leaveTypeId = (int)context.ActionArguments["id"]!;
            var getLeaveTypeDetailRequest = new GetLeaveTypeDetailRequest() { Id = leaveTypeId };
            var leaveType = _mediator.Send(getLeaveTypeDetailRequest);
            if (leaveType == null)
                context.Result = new NotFoundResult();
        }
    }
}
