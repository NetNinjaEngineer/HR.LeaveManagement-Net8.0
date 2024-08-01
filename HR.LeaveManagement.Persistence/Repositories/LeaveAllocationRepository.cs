using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly HRLeaveManagementDbContext _context;

        public LeaveAllocationRepository(HRLeaveManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddAllocations(List<LeaveAllocation> leaveAllocations)
        {
            await _context.AddRangeAsync(leaveAllocations);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AllocationsExists(string userId, int leaveTypeId, int period)
        {
            return await _context.LeaveAllocations.AnyAsync(
                x => x.EmployeeId == userId && x.LeaveTypeId == leaveTypeId && x.Period == period);
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            var leaveAllocations = await _context.LeaveAllocations.Include(x => x.LeaveType)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string id)
        {
            var leaveAllocations = await _context.LeaveAllocations.Where(x => x.EmployeeId == id)
                .Include(x => x.LeaveType)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            var leaveAllocation = await
                _context.LeaveAllocations
                .Include(x => x.LeaveType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return leaveAllocation!;

        }

        public async Task<LeaveAllocation?> GetUserAllocations(string userId, int leaveTypeId)
            => await _context.LeaveAllocations.FirstOrDefaultAsync(x =>
                x.LeaveTypeId == leaveTypeId && x.EmployeeId == userId);
    }
}
