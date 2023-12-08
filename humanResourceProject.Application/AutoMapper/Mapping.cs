using AutoMapper;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.DTO.DTOs;
using humanResourceProject.VM.VMs;

namespace humanResourceProject.Application.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AppUser, AppUserVM>().ReverseMap();
            CreateMap<Company, CompanyVM>().ReverseMap();
            CreateMap<Company, CompanyRegisterDTO>().ReverseMap();

            //CreateMap<>().ReverseMap();
        }
    }
}
