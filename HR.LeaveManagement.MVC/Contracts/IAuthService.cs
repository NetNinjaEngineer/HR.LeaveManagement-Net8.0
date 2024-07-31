using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface IAuthService
    {
        Task<Response<Guid>> Authenticate(string email, string password);

        Task<Response<Guid>> Register(RegisterModel registerModel);

        Task Logout();
    }
}
