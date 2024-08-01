using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface ILeaveAllocationRepository : IGenericRepository<LeaveAllocation>
    {
        Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id);
        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails();
        Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string id);
        Task<bool> AllocationsExists(string userId, int leaveTypeId, int period);
        Task AddAllocations(List<LeaveAllocation> leaveAllocations);
        Task<LeaveAllocation?> GetUserAllocations(string userId, int leaveTypeId);
    }
}
