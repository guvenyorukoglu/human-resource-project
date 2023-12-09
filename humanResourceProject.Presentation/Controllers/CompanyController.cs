
﻿using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace humanResourceProject.Presentation.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyReadService _companyReadService;
        private readonly ICompanyWriteService _companyWriteService;

        public CompanyController(ICompanyReadService companyReadService, ICompanyWriteService companyWriteService)
        {
            _companyReadService = companyReadService;
            _companyWriteService = companyWriteService;
        }

        public async Task<IActionResult> Index()
        {
            var company= await _companyReadService.GetFilteredList(
                select: x => new CompanyVM()
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    NumberOfEmployees = x.NumberOfEmployees,
                    TaxNumber = x.TaxNumber,
                    TaxOffice = x.TaxOffice,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                },
                where: x => x.Id == Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) && x.Status == Status.Active
                );
            return View(company);
        }


        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Insert(CompanyRegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var company = new Company
                {
                    CompanyName = model.CompanyName,
                    Address = model.Address,
                    TaxNumber = model.TaxNumber,
                    TaxOffice = model.TaxOffice,
                    PhoneNumber = model.PhoneNumber,
                    NumberOfEmployees = model.NumberOfEmployees
                };

                _companyWriteService.Insert(company);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        
        public IActionResult Update(Guid id)
        {
            var company = _companyReadService.GetById(id);

            if (company == null)
            {
                return NotFound();
            }

            var companyRegisterDTO = new CompanyRegisterDTO
            {
                CompanyName = company.CompanyName,
                Address = company.Address,
                TaxNumber = company.TaxNumber,
                TaxOffice = company.TaxOffice,
                PhoneNumber = company.PhoneNumber,
                NumberOfEmployees = company.NumberOfEmployees
            };

            return View(companyRegisterDTO);
        }


        [HttpPost]
        public IActionResult Update(CompanyRegisterDTO model)
        {

            if (ModelState.IsValid)
            {
                var company = new Company
                {
                    CompanyName = model.CompanyName,
                    Address = model.Address,
                    TaxNumber = model.TaxNumber,
                    TaxOffice = model.TaxOffice,
                    PhoneNumber = model.PhoneNumber,
                    NumberOfEmployees = model.NumberOfEmployees
                };

                _companyWriteService.Update(company);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public IActionResult Delete(Guid id)
        {
            return View(_companyReadService.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CompanyVM model)
        {
            await _companyWriteService.Delete(model.Id);
            return RedirectToAction("Home");
        } 

    }
}
