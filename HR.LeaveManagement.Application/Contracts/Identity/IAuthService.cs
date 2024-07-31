using HR.LeaveManagement.Application.Models.Identity;

namespace HR.LeaveManagement.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);

        Task<AuthModel> GetTokenRequestModelAsync(TokenRequestModel model);

        Task SignOutAsync();
    }
}
