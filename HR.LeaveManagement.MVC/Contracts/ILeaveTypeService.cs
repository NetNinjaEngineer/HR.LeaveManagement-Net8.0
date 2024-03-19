﻿using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeVM>> GetLeaveTypes();

        Task<LeaveTypeVM> GetLeaveTypeDetails(int id);

        Task<Response<int>> CreateLeaveType(LeaveTypeVM model);

        //Task<Response<int>> UpdateLeaveType(int id, LeaveTypeVM model);

        Task<Response<int>> DeleteLeaveType(int id);
    }
}
