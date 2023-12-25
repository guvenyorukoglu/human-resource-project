﻿using humanResourceProject.Domain.Enum;
using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize]
    public class LeaveController : Controller
    {
        private readonly HttpClient _httpClient;

        public LeaveController()
        {
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/"); // Local
        }

        [Authorize(Roles = "DepartmentManager,Personel")]
        public async Task<IActionResult> MyLeaves()
        {
            Guid employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _httpClient.GetAsync($"api/Leave/GetLeavesByEmployeeId/{employeeId}");
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var leaves = JsonConvert.DeserializeObject<List<LeavePersonnelVM>>(cont);
                return View(leaves);
            }
            return View();
        }

        [Authorize(Roles = "DepartmentManager,CompanyManager")]
        public async Task<IActionResult> EmployeesLeaves()
        {
            if(User.IsInRole("DepartmentManager"))
            {
                Guid depatmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value);

                var response = await _httpClient.GetAsync($"api/Leave/GetLeavesByDepartmentId/{depatmentId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var leaves = JsonConvert.DeserializeObject<List<LeaveVM>>(cont);
                    return View(leaves);
                }
                return View();
            }
            else if(User.IsInRole("CompanyManager"))
            {
                Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

                var response = await _httpClient.GetAsync($"api/Leave/GetLeavesByCompanyId/{companyId}");
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var leaves = JsonConvert.DeserializeObject<List<LeaveVM>>(cont);
                    return View(leaves);
                }
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult CreateLeave()
        {
            return View(new LeaveDTO()
            {
                EmployeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeave(LeaveDTO model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/Leave", content);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyLeaves));

            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLeave(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/Leave/DeleteLeave/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyLeaves));

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateLeave(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetUpdateLeaveDTO/{id}");

            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var updateLeaveDTO = JsonConvert.DeserializeObject<UpdateLeaveDTO>(cont);
                return View(updateLeaveDTO);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLeave(UpdateLeaveDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Leave", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(MyLeaves));

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

        //LEAVE REQUESTS & CONTROLS
        [Authorize(Roles = "DepartmentManager,CompanyManager")]
        [HttpGet]
        public async Task<IActionResult> ApproveLeave(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetUpdateLeaveDTO/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UpdateLeaveDTO>(content);
            model.LeaveStatus = RequestStatus.Approved;

            var json = JsonConvert.SerializeObject(model);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Leave/UpdateStatus", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(EmployeesLeaves));
            }

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }

        [Authorize(Roles = "DepartmentManager,CompanyManager")]
        [HttpGet]
        public async Task<IActionResult> RejectLeave(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/Leave/GetUpdateLeaveDTO/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");
            
            var content = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<UpdateLeaveDTO>(content);
            model.LeaveStatus = RequestStatus.Rejected;

            var json = JsonConvert.SerializeObject(model);
            var contentDTO = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync($"api/Leave/UpdateStatus", contentDTO);

            if (httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(EmployeesLeaves));

            ModelState.AddModelError(httpResponse.StatusCode.ToString(), "Bir hata oluştu.");
            return View("Error");
        }
    }
}

