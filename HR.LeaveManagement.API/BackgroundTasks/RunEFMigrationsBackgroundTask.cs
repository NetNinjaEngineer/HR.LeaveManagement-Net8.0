
using HR.LeaveManagement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.API.BackgroundTasks
{
    public class RunEFMigrationsBackgroundTask : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RunEFMigrationsBackgroundTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var hrLeaveManagementDbContext = scope.ServiceProvider.GetRequiredService<HRLeaveManagementDbContext>();
            await hrLeaveManagementDbContext.Database.MigrateAsync(stoppingToken);
        }
    }
}
