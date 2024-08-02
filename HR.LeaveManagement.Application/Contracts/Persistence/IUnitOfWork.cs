using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Application.Contracts.Persistence;
public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<T>? Repository<T>() where T : BaseDomainEntity;
    Task<int> SaveAsync();
}
