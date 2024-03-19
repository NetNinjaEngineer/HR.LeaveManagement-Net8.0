using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface IAuthService
    {
        Task<bool> Authenticate(string email, string password);

        Task<bool> Register(RegisterModel registerModel);

        Task Logout();
    }
}
