using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.LeaveServices
{
    public class LeaveReadService : BaseReadService<Leave>, ILeaveReadService
    {
        private readonly IBaseReadRepository<Leave> _leaveReadRepository;
        private readonly IMapper _mapper;
        public LeaveReadService(IBaseReadRepository<Leave> leaveReadRepository, IMapper mapper) : base(leaveReadRepository)
        {
            _leaveReadRepository = leaveReadRepository;
            _mapper = mapper;
        }

        public async Task<List<LeaveVM>> GetAllLeaves()
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 Id = x.Id,
                                                                 LeaveType = x.LeaveType,
                                                                 StartDate = x.StartDateOfLeave,
                                                                 EndDate = x.EndDateOfLeave,
                                                                 EmployeeName = x.Employee.FirstName,
                                                                 EmployeeSurname = x.Employee.LastName,
                                                                 EmployeeId = x.Employee.Id,
                                                                 DepartmentId = x.Employee.Department.Id,
                                                                 ManagerId = (Guid)x.Employee.Manager.ManagerId,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave
                                                             },
                                                             where: x => x.Status != Status.Deleted && x.Status != Status.Inactive,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             //commented out because of the error
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<LeaveDTO> GetLeaveById(Guid id)
        {
            Leave leave = await _leaveReadRepository.GetById(id);
            LeaveDTO leaveDTO = _mapper.Map<LeaveDTO>(leave);
            return leaveDTO;
        }

        public async Task<List<LeaveVM>> GetLeavesByDepartmentId(Guid id)
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 Id = x.Id,
                                                                 LeaveType = x.LeaveType,
                                                                 StartDate = x.StartDateOfLeave,
                                                                 EndDate = x.EndDateOfLeave,
                                                                 EmployeeName = x.Employee.FirstName,
                                                                 EmployeeSurname = x.Employee.LastName,
                                                                 EmployeeId = x.Employee.Id,
                                                                 DepartmentId = x.Employee.Department.Id,
                                                                 ManagerId = (Guid)x.Employee.Manager.ManagerId,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave
                                                             },
                                                             where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.DepartmentId == id,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<List<LeaveVM>> GetLeavesByEmployeeId(Guid id)
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 Id = x.Id,
                                                                 LeaveType = x.LeaveType,
                                                                 StartDate = x.StartDateOfLeave,
                                                                 EndDate = x.EndDateOfLeave,
                                                                 EmployeeName = x.Employee.FirstName,
                                                                 EmployeeSurname = x.Employee.LastName,
                                                                 EmployeeId = x.Employee.Id,
                                                                 DepartmentId = x.Employee.Department.Id,
                                                                 ManagerId = (Guid)x.Employee.Manager.ManagerId,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave
                                                             },
                                                             where: x => (x.Status != Status.Deleted && x.Status != Status.Inactive) && x.Employee.Id == id,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<UpdateLeaveDTO> GetUpdateLeaveDTO(Guid id)
        {
            Leave leave = await _leaveReadRepository.GetById(id);
            UpdateLeaveDTO updateLeaveDTO = _mapper.Map<UpdateLeaveDTO>(leave);
            return updateLeaveDTO;
        }
    }
}
