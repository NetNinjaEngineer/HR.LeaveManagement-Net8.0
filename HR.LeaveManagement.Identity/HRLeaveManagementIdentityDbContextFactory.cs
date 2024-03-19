using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HR.LeaveManagement.Identity
{
    public class HRLeaveManagementIdentityDbContextFactory : IDesignTimeDbContextFactory<HRLeaveManagementIdentityDbContext>
    {
        public HRLeaveManagementIdentityDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HRLeaveManagementIdentityDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("HRLeaveManagementIdentity"));
            return new HRLeaveManagementIdentityDbContext(optionsBuilder.Options);
        }
    }
}
