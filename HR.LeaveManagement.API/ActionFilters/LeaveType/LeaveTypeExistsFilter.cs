using HR.LeaveManagement.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HR.LeaveManagement.API.ActionFilters.LeaveType
{
    public class LeaveTypeExistsFilter : IAsyncActionFilter
    {
        private readonly ILeaveTypeRepository leaveTypeRepository;

        public LeaveTypeExistsFilter(ILeaveTypeRepository leaveTypeRepository)
        {
            this.leaveTypeRepository = leaveTypeRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out object? idObj) && idObj is int id)
            {
                var leaveType = await leaveTypeRepository.Get(id);
                if (leaveType == null)
                {
                    context.Result = new NotFoundObjectResult("The Leave Type Requested is not found .");
                    return;
                }
            }

            await next();
        }
    }
}