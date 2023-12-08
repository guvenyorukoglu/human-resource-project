﻿using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository.BaseRepos;

namespace humanResourceProject.Domain.IRepository.CompanyRepo
{
    public interface ICompanyWriteRepository : IBaseWriteRepository<Company>
    {
    }
}
