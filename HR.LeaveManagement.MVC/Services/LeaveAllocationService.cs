using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IClient client;
        public LeaveAllocationService(ILocalStorageService localStorageService, IClient client) : base(localStorageService, client)
        {
            _storageService = localStorageService;
            this.client = client;
        }

        public async Task<Response<int>> CreateLeaveAllocation(int leaveTypeId)
        {
            try
            {
                var response = new Response<int>();

                CreateLeaveAllocationDto createLeaveAllocationDto = new() { LeaveTypeId = leaveTypeId };

                AddBearerToken(_localStorageService.GetStorageValue<string>("token"));

                var apiResponse = await _client.LeaveAllocationsPOSTAsync(createLeaveAllocationDto);

                if (apiResponse.Succeeded)
                {
                    response.Success = true;
                }
                else
                {
                    foreach (var error in apiResponse.Errors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}
