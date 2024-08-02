using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Persistence.Repositories;
using System.Collections;

namespace HR.LeaveManagement.Persistence;
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly HRLeaveManagementDbContext _context;
    private Hashtable _repositories = [];
    private ILeaveAllocationRepository _leaveAllocationRepository;
    private ILeaveTypeRepository _leaveTypeRepository;
    private ILeaveRequestRepository _leaveRequestRepository;

    public UnitOfWork(HRLeaveManagementDbContext context) => _context = context;

    public ILeaveAllocationRepository LeaveAllocationRepository =>
        _leaveAllocationRepository ??= new LeaveAllocationRepository(_context);
    public ILeaveTypeRepository LeaveTypeRepository =>
        _leaveTypeRepository ??= new LeaveTypeRepository(_context);
    public ILeaveRequestRepository LeaveRequestRepository =>
        _leaveRequestRepository ??= new LeaveRequestRepository(_context);

    public async ValueTask DisposeAsync()
        => await _context.DisposeAsync();

    public IGenericRepository<T>? Repository<T>() where T : BaseDomainEntity
    {
        var type = typeof(T).Name;
        if (!_repositories.Contains(type))
        {
            var repository = new GenericRepository<T>(_context);
            _repositories.Add(type, repository);
            return repository;
        }

        return _repositories[type] as IGenericRepository<T>;
    }

    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
}
