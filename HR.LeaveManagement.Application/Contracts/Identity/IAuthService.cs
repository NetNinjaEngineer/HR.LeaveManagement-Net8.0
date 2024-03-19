using HR.LeaveManagement.Application.Contracts.Identity.Models;

namespace HR.LeaveManagement.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);

        Task<AuthModel> GetTokenRequestModelAsync(TokenRequestModel model);

        Task SignOutAsync();
    }
}
