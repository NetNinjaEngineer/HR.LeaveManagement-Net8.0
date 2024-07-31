using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        private readonly HRLeaveManagementDbContext _context;

        public LeaveRequestRepository(HRLeaveManagementDbContext context) : base(context) => _context = context;

        public async Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool? approved)
        {
            leaveRequest.Approved = approved;
            _context.Entry(leaveRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
        {
            var leaveRequestsWithDetails = await _context.LeaveRequests
                .Include(x => x.LeaveType)
                .ToListAsync();

            return leaveRequestsWithDetails;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string id)
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(x => x.LeaveType)
                .Where(x => x.RequestingEmployeeId == id)
                .ToListAsync();

            return leaveRequests;
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(x => x.LeaveType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return leaveRequest!;
        }
    }
}
