using HR.LeaveManagement.API.ActionFilters.LeaveType;
using HR.LeaveManagement.API.Middlewares;
using Microsoft.OpenApi.Models;

namespace HR.LeaveManagement.API
{
    public static class ApiRegisterationServices
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services)
        {
            services.AddScoped<LeaveTypeExistsFilter>();

            services.AddScoped<LeaveTypeCreateValidationsFilter>();

            services.AddScoped<LeaveRequestCreateValidationsFilter>();

            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("HR.LeaveManagement", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Input a valid token to access this API"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "HR.LeaveManagement"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}
