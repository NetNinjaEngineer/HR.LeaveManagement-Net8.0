using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using System.Text;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveRequestService(
        ILocalStorageService localStorageService,
        IClient client,
        IMapper mapper)
        : BaseHttpService(localStorageService, client), ILeaveRequestService
    {
        public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestVM leaveRequest)
        {
            try
            {
                CreateLeaveRequestDto leaveRequestDto = mapper.Map<CreateLeaveRequestDto>(leaveRequest);
                AddBearerToken(_localStorageService.GetStorageValue<string>("token"));
                var response = await _client.LeaveRequestsPOSTAsync(leaveRequestDto);
                if (response.Succeeded)
                    return new Response<int> { Success = true, Message = response.Message };
                else
                {
                    StringBuilder validationErrorsBuilder = new();
                    foreach (var e in response.Errors)
                        validationErrorsBuilder.AppendLine(e);

                    return new Response<int> { Success = false, ValidationErrors = validationErrorsBuilder.ToString(), Message = response.Message };
                }

            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task DeleteLeaveRequest(int id)
        {
            throw new NotImplementedException();
        }
    }
}
