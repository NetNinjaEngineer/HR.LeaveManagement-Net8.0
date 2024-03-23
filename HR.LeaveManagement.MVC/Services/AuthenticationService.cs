using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.LeaveManagement.MVC.Services
{
    public class AuthenticationService : BaseHttpService, IAuthService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private JwtSecurityTokenHandler JwtSecurityTokenHandler;

        public AuthenticationService(ILocalStorageService localStorageService, IClient client, IHttpContextAccessor contextAccessor) : base(localStorageService, client)
        {
            _contextAccessor = contextAccessor;
            JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            try
            {
                TokenRequestModel authenticateModel = new TokenRequestModel() { Email = email, Password = password };

                AuthModel response = await _client.LoginAsync(authenticateModel);

                if (response.IsAuthenticated && response.Token != string.Empty)
                {
                    var tokenContent = JwtSecurityTokenHandler.ReadJwtToken(response.Token);

                    var claims = ParseClaims(tokenContent);

                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                    await _contextAccessor.HttpContext!.SignInAsync(user, new AuthenticationProperties
                    {
                        IsPersistent = false
                    });

                    _localStorageService.SetStorageValue("token", response.Token);

                    return true;

                }

                return false;

            }
            catch
            {
                return false;
            }
        }

        private List<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            claims.Add(new Claim(ClaimTypes.Role, "roles"));
            return claims;
        }

        public async Task Logout()
        {
            _localStorageService.ClearStorage(new List<string> { "token" });
            await _contextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            try
            {
                AuthModel authModelResponse = await _client.RegisterAsync(registerModel);
                if (authModelResponse.IsAuthenticated && authModelResponse.Token != string.Empty)
                    return await Authenticate(registerModel.Email, registerModel.Password);

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
