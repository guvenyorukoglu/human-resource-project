﻿using AutoMapper;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.LeaveRepo;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Application.Services.Concrete.LeaveServices
{
    public class LeaveReadService : BaseReadService<Leave>, ILeaveReadService
    {
        private readonly IBaseReadRepository<Leave> _baseReadRepository;
        private readonly ILeaveReadRepository _leaveReadRepository;
        private readonly IMapper _mapper;
        public LeaveReadService(IBaseReadRepository<Leave> readRepository, ILeaveReadRepository leaveReadRepository, IMapper mapper) : base(readRepository)
        {
            _baseReadRepository = readRepository;
            _leaveReadRepository = leaveReadRepository;
            _mapper = mapper;
        }

        public async Task<List<LeaveVM>> GetAllLeaves()
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 LeaveType = x.LeaveType,
                                                                 StartDate = x.StartDateOfLeave,
                                                                 EndDate = x.EndDateOfLeave,
                                                                 EmployeeName = x.Employee.FirstName,
                                                                 EmployeeSurname = x.Employee.LastName,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave
                                                             },
                                                             where: x => x.Status != Status.Deleted || x.Status != Status.Inactive,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<LeaveDTO> GetLeaveById(Guid id)
        {
            Leave leave = await _baseReadRepository.GetById(id);
            LeaveDTO leaveDTO = _mapper.Map<LeaveDTO>(leave);
            return leaveDTO;
        }

        public async Task<List<LeaveVM>> GetLeavesByDepartmentId(Guid id)
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 LeaveType = x.LeaveType,
                                                                 StartDate = x.StartDateOfLeave,
                                                                 EndDate = x.EndDateOfLeave,
                                                                 EmployeeName = x.Employee.FirstName,
                                                                 EmployeeSurname = x.Employee.LastName,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave
                                                             },
                                                             where: x => (x.Status != Status.Deleted || x.Status != Status.Inactive) && x.Employee.DepartmentId == id,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }

        public async Task<List<LeaveVM>> GetLeavesByEmployeeId(Guid id)
        {
            List<LeaveVM>? leaves = await _leaveReadRepository.GetFilteredList(
                                                             select: x => new LeaveVM
                                                             {
                                                                 LeaveType = x.LeaveType,
                                                                 StartDate = x.StartDateOfLeave,
                                                                 EndDate = x.EndDateOfLeave,
                                                                 EmployeeName = x.Employee.FirstName,
                                                                 EmployeeSurname = x.Employee.LastName,
                                                                 LeaveStatus = x.LeaveStatus,
                                                                 DaysOfLeave = x.DaysOfLeave
                                                             },
                                                             where: x => (x.Status != Status.Deleted || x.Status != Status.Inactive) && x.Employee.Id == id,
                                                             orderBy: x => x.OrderByDescending(x => x.CreateDate),
                                                             include: x => x.Include(x => x.Employee));
            return leaves;
        }
    }
}
