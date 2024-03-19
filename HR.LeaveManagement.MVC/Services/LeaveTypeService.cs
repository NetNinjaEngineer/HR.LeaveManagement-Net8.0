using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using System.Net.Http.Headers;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveTypeService : BaseHttpService, ILeaveTypeService
    {
        private readonly IMapper _mapper;

        public LeaveTypeService(ILocalStorageService localStorageService, IClient client, IMapper mapper) : base(localStorageService, client)
        {
            _mapper = mapper;
            _client.HttpClient.DefaultRequestHeaders.Clear();
            _client.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Response<int>> CreateLeaveType(CreateLeaveTypeVM model)
        {
            try
            {
                var response = new Response<int>();

                CreateLeaveTypeDto createLeaveTypeDto = _mapper.Map<CreateLeaveTypeVM, CreateLeaveTypeDto>(model);

                var apiResponse = await _client.LeaveTypesPOSTAsync(createLeaveTypeDto);

                if (apiResponse.Succeeded)
                {
                    response.Success = true;
                    response.Message = apiResponse.Message;
                }
                else
                    foreach (var error in apiResponse.Errors)
                        response.ValidationErrors += error + Environment.NewLine;

                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<Response<int>> DeleteLeaveType(int id)
        {
            try
            {
                var response = new Response<int>();

                var apiResponse = await _client.LeaveTypesDELETEAsync(id);

                if (apiResponse.Succeeded)
                {
                    response.Success = true;
                    response.Message = apiResponse.Message;
                }
                else
                    foreach (var error in apiResponse.Errors)
                        response.ValidationErrors += error + Environment.NewLine;

                return response;

            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<LeaveTypeVM> GetLeaveTypeDetails(int id)
        {
            var leaveType = await _client.LeaveTypesGETAsync(id);
            var leaveTypeVM = _mapper.Map<LeaveTypeVM>(leaveType);
            return leaveTypeVM;
        }

        public async Task<List<LeaveTypeVM>> GetLeaveTypes()
        {
            var leaveTypes = await _client.LeaveTypesAllAsync();
            var leaveTypesVM = _mapper.Map<IEnumerable<LeaveTypeDto>, IEnumerable<LeaveTypeVM>>(leaveTypes);
            return leaveTypesVM.ToList();
        }

        public async Task<Response<int>> UpdateLeaveType(int id, LeaveTypeVM model)
        {
            try
            {
                LeaveTypeDto leaveTypeDto = _mapper.Map<LeaveTypeDto>(model);
                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}
