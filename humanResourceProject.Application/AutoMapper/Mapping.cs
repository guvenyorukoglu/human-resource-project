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

            CreateMap<Department, DepartmentVM>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();

            CreateMap<Job, JobVM>().ReverseMap();
            CreateMap<Job, JobDTO>().ReverseMap();

            CreateMap<Advance, AdvanceVM>().ReverseMap();
            CreateMap<Advance, AdvanceDTO>().ReverseMap();

            CreateMap<Expense, ExpenseVM>().ReverseMap();
            CreateMap<Expense, ExpenseDTO>().ReverseMap();

            CreateMap<Leave, LeaveVM>().ReverseMap();
            CreateMap<Leave, LeaveDTO>().ReverseMap();

            //CreateMap<>().ReverseMap();
        }
    }
}
