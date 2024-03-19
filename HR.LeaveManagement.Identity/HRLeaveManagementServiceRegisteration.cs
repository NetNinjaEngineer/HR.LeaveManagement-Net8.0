using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Identity
{
    public static class HRLeaveManagementServiceRegisteration
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<HRLeaveManagementIdentityDbContext>()
                .AddDefaultTokenProviders();

            return services;

        }
    }
}
