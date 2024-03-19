using HR.LeaveManagement.API.ActionFilters.LeaveType;

namespace HR.LeaveManagement.API
{
    public static class ApiRegisterationServices
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services)
        {
            services.AddScoped<LeaveTypeExistsFilter>();

            services.AddScoped<LeaveTypeCreateValidationsFilter>();

            services.AddScoped<LeaveRequestCreateValidationsFilter>();

            return services;
        }
    }
}
