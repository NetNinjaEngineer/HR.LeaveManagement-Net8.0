using HR.LeaveManagement.MVC.Contracts;
using System.Net.Http.Headers;

namespace HR.LeaveManagement.MVC.Services.Base
{
    public class BaseHttpService
    {
        protected readonly IClient _client;
        protected readonly ILocalStorageService _localStorageService;

        public BaseHttpService(IClient client, ILocalStorageService localStorageService)
        {
            _client = client;
            _localStorageService = localStorageService;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
        {
            if (exception.StatusCode == 400)
            {
                return new Response<Guid> { Message = "validation errors have occured.", Success = false };
            }
            else if (exception.StatusCode == 404)
            {
                return new Response<Guid> { Message = "The Requested Item could not be found.", Success = false };
            }
            else
            {
                return new Response<Guid> { Message = "Something went wrong, please try again.", Success = false };
            }
        }

        protected void AddBearerToken()
        {
            if (_localStorageService.Exists("token"))
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer",
                    _localStorageService.GetStorageValue<string>("token"));
        }
    }
}
