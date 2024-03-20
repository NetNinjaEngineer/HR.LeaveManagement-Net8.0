using HR.LeaveManagement.Application.Contracts.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Contracts.Identity
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync();
    }
}
