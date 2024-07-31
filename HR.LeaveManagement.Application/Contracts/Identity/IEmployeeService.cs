using HR.LeaveManagement.Application.Models.Identity;

namespace HR.LeaveManagement.Application.Contracts.Identity
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeById(string id);
    }
}
