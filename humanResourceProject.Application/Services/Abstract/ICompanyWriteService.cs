﻿using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.DTO.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Abstract
{
    public interface ICompanyWriteService : IBaseWriteService<Company>
    {
        Task Register(CompanyRegisterDTO model);
    }
}