using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HR.LeaveManagement.Identity.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Employee> GetEmployeeById(string id)
        {
            var employee = await _userManager.FindByIdAsync(id);
            return new Employee
            {
                EmployeeId = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email
            };
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Employee");
            return users.Select(user => new Employee
            {
                EmployeeId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList();
        }
    }
}
