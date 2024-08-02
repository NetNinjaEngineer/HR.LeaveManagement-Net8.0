using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Application.Contracts.Persistence;
public interface IUnitOfWork : IAsyncDisposable
{
    public ILeaveAllocationRepository LeaveAllocationRepository { get; }
    public ILeaveTypeRepository LeaveTypeRepository { get; }
    public ILeaveRequestRepository LeaveRequestRepository { get; }
    IGenericRepository<T>? Repository<T>() where T : BaseDomainEntity;

    Task<int> SaveAsync();
}
