using AutoMapper;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLeaveTypeDto, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();
            CreateMap<RegisterViewModel, RegisterModel>();
            CreateMap<CreateLeaveRequestVM, CreateLeaveRequestDto>().ReverseMap();
        }
    }
}
