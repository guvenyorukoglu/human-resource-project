using humanResourceProject.Models.DTOs;
using humanResourceProject.Models.VMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace humanResourceProject.Presentation.Controllers
{
    [Authorize(Roles = "SiteManager, CompanyManager, Manager, Personel")]
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private List<JobVM> jobs;
        List<DepartmentVM> departments;
        List<ManagerVM> managers;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);

            jobs = new List<JobVM>();
            departments = new List<DepartmentVM>();
            managers = new List<ManagerVM>();
        }

        public async Task<IActionResult> Employees()
        {
            //Guid id = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);
            var json = JsonConvert.SerializeObject(companyId);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/AppUser/GetEmployeesByCompanyId/", content);
            if (response.IsSuccessStatusCode)
            {
                var cont = await response.Content.ReadAsStringAsync();
                var employees = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);

                // Kendi bilgilerini çıkartır
                //employees = employees.Except(employees.Where(x => x.Id == id)).ToList();

                return View(employees);

            }
            return View("Error");

        }

        [HttpGet]
        public async Task<IActionResult> CreatePersonel()
        {

            Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);
            HttpResponseMessage messageJobs = await _httpClient.GetAsync($"api/Job/GetJobsByCompanyId/{companyId}");
            HttpResponseMessage messageDepartments = await _httpClient.GetAsync($"api/Department/GetDepartmentsByCompanyId/{companyId}");
            HttpResponseMessage messageManagers = await _httpClient.GetAsync($"api/AppUser/GetManagersByCompanyId/{companyId}");

            if (messageJobs.IsSuccessStatusCode && messageDepartments.IsSuccessStatusCode && messageManagers.IsSuccessStatusCode)
            {
                var jobsResponse = await messageJobs.Content.ReadAsStringAsync();
                dynamic jobList = JsonConvert.DeserializeObject(jobsResponse);

                foreach (var job in jobList)
                {
                    jobs.Add(new JobVM()
                    {
                        Id = job.id,
                        Title = job.title,
                        Description = job.description
                    });
                }

                var departmentsResponse = await messageDepartments.Content.ReadAsStringAsync();
                dynamic departmentList = JsonConvert.DeserializeObject(departmentsResponse);

                foreach (var department in departmentList)
                {
                    departments.Add(new DepartmentVM()
                    {
                        Id = department.id,
                        DepartmentName = department.departmentName,
                        Description = department.description
                    });
                }

                var managersResponse = await messageManagers.Content.ReadAsStringAsync();
                dynamic managerList = JsonConvert.DeserializeObject(managersResponse);

                foreach (var manager in managerList)
                {
                    managers.Add(new ManagerVM()
                    {
                        Id = manager.id,
                        FullName = manager.fullName
                    });
                }

                return View(new CreateEmployeeDTO()
                {
                    Departments = departments,
                    Jobs = jobs,
                    Managers = managers,
                    CompanyId = companyId
                });
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonel(CreateEmployeeDTO model, string JobsList, string DepartmentsList, string ManagersList)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState["BloodGroup"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Kan grubunu seçiniz!");
                }
                if (ModelState["Gender"].Errors.Count > 0)
                {
                    ModelState.AddModelError(string.Empty, "Cinsiyet seçiniz!");
                }
                model.Jobs = JsonConvert.DeserializeObject<List<JobVM>>(JobsList);
                model.Departments = JsonConvert.DeserializeObject<List<DepartmentVM>>(DepartmentsList);
                model.Managers = JsonConvert.DeserializeObject<List<ManagerVM>>(ManagersList);
                ModelState.AddModelError(string.Empty, "Lütfen tüm verileri doğru girdiğinizden emin olunuz!");
                return View(model);
            }
            if(model.JobId == Guid.Empty || model.DepartmentId == Guid.Empty)
            {
                model.Jobs = JsonConvert.DeserializeObject<List<JobVM>>(JobsList);
                model.Departments = JsonConvert.DeserializeObject<List<DepartmentVM>>(DepartmentsList);
                model.Managers = JsonConvert.DeserializeObject<List<ManagerVM>>(ManagersList);
                ModelState.AddModelError(string.Empty, "Lütfen pozisyon ve departman bilgisinin girildiğinden emin olunuz!");
                return View(model);
            }
            model.ImagePath = model.Gender == Domain.Enum.Gender.Female ? "https://ik.imagekit.io/7ypp4olwr/femaledefault.png?tr=h-200,w-200" : "https://ik.imagekit.io/7ypp4olwr/maledefault.png?tr=h-200,w-200";

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            if (model.UserRole.ToString() == "Personel")
            {
                var response = await _httpClient.PostAsync($"api/AppUser", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessCreateProfileMessage"] = "Şirketinize Personel Eklenmiştir.";
                    return RedirectToAction(nameof(Employees));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lütfen girdiğiniz verileri kontrol ediniz!");
                    model.Jobs = JsonConvert.DeserializeObject<List<JobVM>>(JobsList);
                    model.Departments = JsonConvert.DeserializeObject<List<DepartmentVM>>(DepartmentsList);
                    model.Managers = JsonConvert.DeserializeObject<List<ManagerVM>>(ManagersList);
                    return View(model);
                }

            }
            else if (model.UserRole.ToString() == "Manager")
            {
                var response = await _httpClient.PostAsync($"api/AppUser/CreatePersonelManager", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessCreateProfileMessage"] = "Şirketinize Personel Eklenmiştir.";
                    return RedirectToAction(nameof(Employees));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Lütfen girdiğiniz verileri kontrol ediniz!");
                    model.Jobs = JsonConvert.DeserializeObject<List<JobVM>>(JobsList);
                    model.Departments = JsonConvert.DeserializeObject<List<DepartmentVM>>(DepartmentsList);
                    model.Managers = JsonConvert.DeserializeObject<List<ManagerVM>>(ManagersList);
                    return View(model);
                }
            }
            model.Jobs = JsonConvert.DeserializeObject<List<JobVM>>(JobsList);
            model.Departments = JsonConvert.DeserializeObject<List<DepartmentVM>>(DepartmentsList);
            model.Managers = JsonConvert.DeserializeObject<List<ManagerVM>>(ManagersList);
            ModelState.AddModelError(string.Empty, "Lütfen tüm verileri doğru girdiğinizden emin olunuz!");
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/AppUser/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessDeleteProfileMessage"] = "Şirketinize Personel silinmiştir.";
                return RedirectToAction("Employees");
            }

            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/AppUser/GetUpdateUserDTO/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<UpdateUserDTO>(content);

                return View(model);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(UpdateUserDTO model, string JobsList, string DepartmentsList, string ManagersList)
        {
            if (!ModelState.IsValid)
            {
                TempData["modelinvalid"] = "modelinvalid";

                // Listeleri tekrar doldur
                model.Jobs = JsonConvert.DeserializeObject<List<JobVM>>(JobsList);
                model.Departments = JsonConvert.DeserializeObject<List<DepartmentVM>>(DepartmentsList);
                model.Managers = JsonConvert.DeserializeObject<List<ManagerVM>>(ManagersList);


                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/AppUser", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUpdateEmployeeMessage"] = "Personel profili güncellenmiştir.";
                return RedirectToAction(nameof(Employees));
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                // Listeleri tekrar doldur
                model.Jobs = JsonConvert.DeserializeObject<List<JobVM>>(JobsList);
                model.Departments = JsonConvert.DeserializeObject<List<DepartmentVM>>(DepartmentsList);
                model.Managers = JsonConvert.DeserializeObject<List<ManagerVM>>(ManagersList);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> FireEmployee(Guid employeeId, string terminationReason, DateTime terminationDate)
        {
            var responsePossession = await _httpClient.GetAsync($"api/Possession/GetPossessionsByEmployeeId/{employeeId}");
            if (responsePossession.IsSuccessStatusCode)
            {
                var contentPossession = await responsePossession.Content.ReadAsStringAsync();
                var possessions = JsonConvert.DeserializeObject<List<PersonelPossessionVM>>(contentPossession);
                if (possessions.Count > 0)
                {
                    return StatusCode(400, "possessions");
                }


                FireEmployeeDTO model = new FireEmployeeDTO()
                {
                    EmployeeId = employeeId,
                    ReasonForTermination = terminationReason,
                    TerminationDate = terminationDate
                };

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/AppUser/FireEmployee/", content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok("success");
                }
                return View("Error");
            }
            else
            {

                return View("Error");

            }
        }

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            var responseLeave = await _httpClient.GetAsync($"api/Leave/FillDashboardLeaveVM/{userId}");
            var responseAdvance = await _httpClient.GetAsync($"api/Advance/FillDashboardAdvanceVM/{userId}");
            var responseExpense = await _httpClient.GetAsync($"api/Expense/FillDashboardExpenseVM/{userId}");
            var responseCompany = await _httpClient.GetAsync($"api/Company/GetCompanyVM/{companyId}");
            var responseAppUser = await _httpClient.GetAsync($"api/AppUser/GetAppUserVM/{userId}");

            if (responseLeave.IsSuccessStatusCode && responseAdvance.IsSuccessStatusCode && responseExpense.IsSuccessStatusCode && responseCompany.IsSuccessStatusCode)
            {
                var contentLeave = await responseLeave.Content.ReadAsStringAsync();
                var dashboardLeaveVM = JsonConvert.DeserializeObject<DashboardLeaveVM>(contentLeave);

                var contentAdvance = await responseAdvance.Content.ReadAsStringAsync();
                var dashboardAdvanceVM = JsonConvert.DeserializeObject<DashboardAdvanceVM>(contentAdvance);

                var contentExpense = await responseExpense.Content.ReadAsStringAsync();
                var dashboardExpenseVM = JsonConvert.DeserializeObject<DashboardExpenseVM>(contentExpense);

                var contentCompany = await responseCompany.Content.ReadAsStringAsync();
                var dashboardCompanyVM = JsonConvert.DeserializeObject<CompanyVM>(contentCompany);

                var contentAppUser = await responseAppUser.Content.ReadAsStringAsync();
                var dashboardAppUserVM = JsonConvert.DeserializeObject<AppUserVM>(contentAppUser);


                DashboardVM dashboardVM = new DashboardVM()
                {
                    MyLeaves = dashboardLeaveVM.MyLeaves,
                    MyAdvances = dashboardAdvanceVM.MyAdvances,
                    MyExpenses = dashboardExpenseVM.MyExpenses,
                    Company = dashboardCompanyVM,
                    AppUser = dashboardAppUserVM,
                    LeavesToBeCompletedByManager = dashboardLeaveVM.LeavesToBeCompletedByManager,
                    AdvancesToBeCompletedByManager = dashboardAdvanceVM.AdvancesToBeCompletedByManager,
                    ExpensesToBeCompletedByManager = dashboardExpenseVM.ExpensesToBeCompletedByManager,
                    MyPendingLeavesCount = dashboardLeaveVM.MyLeaves.Where(x => x.LeaveStatus == Domain.Enum.RequestStatus.Pending).Count(),
                    MyPendingAdvancesCount = dashboardAdvanceVM.MyAdvances.Where(x => x.AdvanceStatus == Domain.Enum.RequestStatus.Pending).Count(),
                    MyPendingExpensesCount = dashboardExpenseVM.MyExpenses.Where(x => x.ExpenseStatus == Domain.Enum.RequestStatus.Pending).Count(),


                };

                return View(dashboardVM);
            }

            return View("Error");
        }

        //[HttpGet]
        //public async Task<IActionResult> EditProfile(Guid id)
        //{
        //    var response = await _httpClient.GetAsync($"api/AppUser/GetUpdateProfileDTO/{id}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        var model = JsonConvert.DeserializeObject<UpdateProfileDTO>(content);

        //        return View(model);
        //    }
        //    return View("Error");
        //}

        [HttpPost]
        public async Task<IActionResult> ProfileEmployee(UpdateProfileDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Result"] = "modelinvalid";
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/AppUser/UpdateProfile", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessUpdateProfileMessage"] = "Profiliniz güncellenmiştir.";
                return RedirectToAction("ProfileEmployee", "Employee");
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProfileEmployee(Guid Id)
        {
            if (!User.IsInRole("SiteManager"))
            {
                var employeeId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var response = await _httpClient.GetAsync($"api/AppUser/ProfileEmployee/{employeeId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<UpdateProfileDTO>(content);
                    return View(model);
                }
                return View("Error");
            }
            else
            {
                var companyManagerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var response = await _httpClient.GetAsync($"api/AppUser/ProfileCompanyManager/{companyManagerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<UpdateProfileDTO>(content);
                    return View(model);
                }
                return View("Error");
            }
        }
    }


}
