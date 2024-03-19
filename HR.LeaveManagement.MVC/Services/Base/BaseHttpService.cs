using HR.LeaveManagement.MVC.Contracts;
using System.Net.Http.Headers;

namespace HR.LeaveManagement.MVC.Services.Base
{
    public class BaseHttpService
    {
        protected readonly ILocalStorageService _localStorageService;

        protected readonly IClient _client;

        public BaseHttpService(ILocalStorageService localStorageService, IClient client)
        {
            _localStorageService = localStorageService;
            _client = client;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
        {
            if (ex.StatusCode == 400)
            {
                return new Response<Guid> { Message = "Validation errors have occured", Success = false, ValidationErrors = ex.Response };
            }
            else if (ex.StatusCode == 404)
            {
                return new Response<Guid> { Message = "The requested item could not be founded!.", Success = false };
            }
            return new Response<Guid> { Message = "Something went wrong please try again", Success = false };
        }

        protected void AddBearerToken(string token)
        {
            if (!_localStorageService.Exists(token))
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
