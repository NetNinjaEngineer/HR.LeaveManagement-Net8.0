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
        public async Task ApproveLeaveRequest(int id, bool approved)
        {
            try
            {
                AddBearerToken(_localStorageService.GetStorageValue<string>("token"));
                var request = new ChangeLeaveRequestApprovalDto { Id = id, Approved = approved };
                await _client.ChangeApprovalAsync(id, request);
            }
            catch (Exception)
            {

                throw;
            }
        }

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
            await _client.LeaveRequestsDELETEAsync(id);
        }

        public async Task<AdminLeaveRequestViewVM> GetAdminLeaveRequestList()
        {
            AddBearerToken(_localStorageService.GetStorageValue<string>("token"));
            var leaveRequests = await _client.LeaveRequestsGET2Async(isLoggedInUser: false);
            return new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(x => x.Approved == true),
                LeaveRequests = mapper.Map<List<LeaveRequestVM>>(leaveRequests),
                PendingRequests = leaveRequests.Count(x => x.Approved == null),
                RejectedRequests = leaveRequests.Count(x => x.Approved == false)
            };
        }

        public async Task<EmployeeLeaveRequestViewVM> GetEmployeeLeaveRequests()
        {
            AddBearerToken(_localStorageService.GetStorageValue<string>("token"));
            var leaveRequests = await _client.LeaveRequestsGET2Async(isLoggedInUser: true);
            return new EmployeeLeaveRequestViewVM
            {
                LeaveRequests = mapper.Map<List<LeaveRequestVM>>(leaveRequests)
            };
        }

        public async Task<LeaveRequestVM> GetLeaveRequest(int id)
        {
            AddBearerToken(_localStorageService.GetStorageValue<string>("token"));
            return mapper.Map<LeaveRequestVM>(await _client.LeaveRequestsGETAsync(id));
        }
    }
}
