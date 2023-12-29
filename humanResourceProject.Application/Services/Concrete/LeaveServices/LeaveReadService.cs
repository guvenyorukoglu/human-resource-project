using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.LeaveServices
{
    public class LeaveReadService : BaseReadService<Leave>, ILeaveReadService
    {
        private readonly IBaseReadRepository<Leave> _leaveReadRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public LeaveReadService(IBaseReadRepository<Leave> leaveReadRepository, IMapper mapper, UserManager<AppUser> userManager) : base(leaveReadRepository)
        {
            _leaveReadRepository = leaveReadRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<LeaveDTO> GetLeaveById(Guid id)
        {
            Leave leave = await _leaveReadRepository.GetById(id);
            LeaveDTO leaveDTO = _mapper.Map<LeaveDTO>(leave);
            return leaveDTO;
        }

        public async Task<List<LeaveVM>> GetAllLeaves()
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 Id = x.Id,
                                                                 LeaveType = x.LeaveType,
                                                                 StartDateOfLeave = x.StartDateOfLeave,
                                                                 EndDateOfLeave = x.EndDateOfLeave,
                                                                 FirstName = x.Employee.FirstName,
                                                                 LastName = x.Employee.LastName,
                                                                 EmployeeId = x.Employee.Id,
                                                                 ManagerId = (Guid)x.Employee.ManagerId,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave,
                                                                 CreateDate = x.CreateDate,
                                                                 Explanation = x.Explanation,
                                                                 LeaveNo = x.LeaveNo
                                                             },
                                                             where: x => x.Status != Status.Deleted && x.Status != Status.Inactive,
                                                             //commented out because of the error
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<List<LeaveVM>> GetLeavesByCompanyId(Guid id)
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                            select: x => new LeaveVM
                                                            {
                                                                Id = x.Id,
                                                                LeaveType = x.LeaveType,
                                                                StartDateOfLeave = x.StartDateOfLeave,
                                                                EndDateOfLeave = x.EndDateOfLeave,
                                                                FirstName = x.Employee.FirstName,
                                                                LastName = x.Employee.LastName,
                                                                EmployeeId = x.Employee.Id,
                                                                LeaveStatus = x.LeaveStatus,
                                                                DaysOfLeave = x.DaysOfLeave,
                                                                ManagerId = (Guid)x.Employee.ManagerId,
                                                                CreateDate = x.CreateDate,
                                                                Explanation = x.Explanation,
                                                                LeaveNo = x.LeaveNo
                                                            },
                                                            where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.CompanyId == id,
                                                            orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                            include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<List<LeaveVM>> GetLeavesByManagerId(Guid id)
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 Id = x.Id,
                                                                 LeaveType = x.LeaveType,
                                                                 StartDateOfLeave = x.StartDateOfLeave,
                                                                 EndDateOfLeave = x.EndDateOfLeave,
                                                                 FirstName = x.Employee.FirstName,
                                                                 LastName = x.Employee.LastName,
                                                                 EmployeeId = x.Employee.Id,
                                                                 ManagerId = (Guid)x.Employee.ManagerId,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave,
                                                                 CreateDate = x.CreateDate,
                                                                 Explanation = x.Explanation,
                                                                 LeaveNo = x.LeaveNo
                                                             },
                                                             where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.ManagerId == id,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<List<LeavePersonnelVM>> GetLeavesByEmployeeId(Guid id)
        {
            List<LeavePersonnelVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeavePersonnelVM
                                                             {
                                                                 Id = x.Id,
                                                                 LeaveType = x.LeaveType,
                                                                 StartDateOfLeave = x.StartDateOfLeave,
                                                                 EndDateOfLeave = x.EndDateOfLeave,
                                                                 EmployeeId = x.Employee.Id,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave,
                                                                 CreateDate = x.CreateDate,
                                                                 Explanation = x.Explanation,
                                                                 ManagerFullName = $"{x.Employee.Manager.FirstName} {x.Employee.Manager.LastName}",
                                                                 LeaveNo = x.LeaveNo
                                                             },
                                                             where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Id == id,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee).ThenInclude(x => x.Manager));
            return leaves;
        }

        public async Task<UpdateLeaveDTO> GetUpdateLeaveDTO(Guid id)
        {
            Leave leave = await _leaveReadRepository.GetById(id);
            UpdateLeaveDTO updateLeaveDTO = _mapper.Map<UpdateLeaveDTO>(leave);
            return updateLeaveDTO;
        }

        public async Task<DashboardLeaveVM> FillDashboardLeaveVM(Guid id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id.ToString());

            List<DashboardLeavesVM> dashboardMyLeavesVMs = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new DashboardLeavesVM
                                                             {
                                                                 LeaveNo = x.LeaveNo,
                                                                 DaysOfLeave = x.DaysOfLeave,
                                                                 CreateDate = x.CreateDate,
                                                                 LeaveStatus = x.LeaveStatus,
                                                             },
                                                             where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Id == id,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));

            List<DashboardLeavesVM> dashboardLeavesToBeCompletedByManagerVM;

            if (_userManager.IsInRoleAsync(appUser, "CompanyManager").Result)
            {
                dashboardLeavesToBeCompletedByManagerVM = await _leaveReadRepository.GetFilteredList(
                                                            select: x => new DashboardLeavesVM
                                                            {
                                                                LeaveNo = x.LeaveNo,
                                                                DaysOfLeave = x.DaysOfLeave,
                                                                CreateDate = x.CreateDate,
                                                                LeaveStatus = x.LeaveStatus,
                                                            },
                                                            where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.CompanyId == appUser.CompanyId && x.LeaveStatus == RequestStatus.Pending,
                                                            orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                            include: x => x.Include(x => x.Employee));
            }
            else
            {
                dashboardLeavesToBeCompletedByManagerVM = await _leaveReadRepository.GetFilteredList(
                                                            select: x => new DashboardLeavesVM
                                                            {
                                                                LeaveNo = x.LeaveNo,
                                                                DaysOfLeave = x.DaysOfLeave,
                                                                CreateDate = x.CreateDate,
                                                                LeaveStatus = x.LeaveStatus,
                                                            },
                                                            where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.ManagerId == id && x.LeaveStatus == RequestStatus.Pending,
                                                            orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                            include: x => x.Include(x => x.Employee));
            }
            return new DashboardLeaveVM() { MyLeaves = dashboardMyLeavesVMs, LeavesToBeCompletedByManager = dashboardLeavesToBeCompletedByManagerVM };
        }

        public async Task<LeaveDTO> GetLeaveDTO(Guid employeeId)
        {
            AppUser employee = _userManager.FindByIdAsync(employeeId.ToString()).Result;
            AppUser managerOfEmployee = _userManager.FindByIdAsync(employee.ManagerId.ToString()).Result;
            LeaveDTO leaveDTO = new LeaveDTO()
            {
                EmployeeId = employee.Id,
                ManagerFullName = managerOfEmployee.FirstName + " " + managerOfEmployee.LastName,
                ManagerEmail = managerOfEmployee.Email
            };
            return leaveDTO;
        }
    }
}
