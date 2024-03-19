using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Identity
{
    public class HRLeaveManagementIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public HRLeaveManagementIdentityDbContext(DbContextOptions<HRLeaveManagementIdentityDbContext> options) : base(options)
        {

        }
    }
}
