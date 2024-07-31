using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;

namespace HR.LeaveManagement.MVC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllersWithViews();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<ILeaveTypeService, LeaveTypeService>();
        services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();
        services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        services.AddSingleton<ILocalStorageService, LocalStorageService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IAuthService, AuthenticationService>();
        services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Home/Error");
            });

        services.AddHttpClient<IClient, Client>(opt =>
            opt.BaseAddress = new Uri(configuration["ApiBaseUrl"]!));

        return services;
    }
}
