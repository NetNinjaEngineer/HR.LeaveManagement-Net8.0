﻿using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Identity
{
    public class HRLeaveManagementIdentityDbContext(
        DbContextOptions<HRLeaveManagementIdentityDbContext> options)
        : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(HRLeaveManagementIdentityDbContext).Assembly);

        }
    }
}
