using AutoMapper;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;

namespace humanResourceProject.Application.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AppUser, AppUserVM>().ReverseMap();
            CreateMap<Company, CompanyVM>().ReverseMap();
            CreateMap<Company, CompanyRegisterDTO>().ReverseMap();
            CreateMap<Company, UpdateCompanyDTO>().ReverseMap();

            CreateMap<AppUser, UpdateUserDTO>().ReverseMap();
            CreateMap<AppUser, UserRegisterDTO>().ReverseMap();
            CreateMap<AppUser, PersonelVM>().ReverseMap();
            CreateMap<AppUser, LoginDTO>().ReverseMap();

            //CreateMap<>().ReverseMap();
        }
    }
}
