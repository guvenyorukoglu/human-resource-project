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

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new Uri("https://monitoreaseapi.azurewebsites.net"); // Azure
            _httpClient.BaseAddress = new Uri("https://localhost:7255/");
            jobs = new List<JobVM>();
        }

        public async Task<IActionResult> Employees()
        {
            if (User.IsInRole("CompanyManager")) // Şirket Yöneticisi ise tüm personelleri getirir
            {
                Guid companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);
                var json = JsonConvert.SerializeObject(companyId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/AppUser/GetEmployeesByCompanyId/", content);
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                    return View(employees);

                }
                return View();
            }
            else if (User.IsInRole("Manager")) // Departman Yöneticisi ise departmanındaki personelleri getirir
            {
                Guid departmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value);
                var json = JsonConvert.SerializeObject(departmentId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/AppUser/GetEmployeesByDepartmentId/", content);
                if (response.IsSuccessStatusCode)
                {
                    var cont = await response.Content.ReadAsStringAsync();
                    var employees = JsonConvert.DeserializeObject<List<PersonelVM>>(cont);
                    return View(employees);

                }
                return View();
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreatePersonel()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/Job/GetAllJobs/");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                dynamic jobList = JsonConvert.DeserializeObject(apiResponse);

                foreach (var job in jobList)
                {
                    jobs.Add(new JobVM()
                    {
                        Id = job.id,
                        Title = job.title,
                        Description = job.description
                    });
                }

                if (User.IsInRole("CompanyManager"))
                {
                    HttpResponseMessage message = await _httpClient.GetAsync($"api/Department/GetDepartmentsByCompanyId/{Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value)}");
                    if (message.IsSuccessStatusCode)
                    {
                        var apiResponse2 = await message.Content.ReadAsStringAsync();
                        dynamic departmentList = JsonConvert.DeserializeObject(apiResponse2);

                        List<DepartmentVM> departments = new List<DepartmentVM>();

                        foreach (var department in departmentList)
                        {
                            departments.Add(new DepartmentVM()
                            {
                                Id = department.id,
                                DepartmentName = department.departmentName,
                                Description = department.description
                            });
                        }

                        return View(new CreateEmployeeDTO()
                        {
                            Departments = departments,
                            Jobs = jobs
                        });
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else if (User.IsInRole("Manager"))
                    return View(new CreateEmployeeDTO()
                    {
                        DepartmentId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "DepartmentId").Value),
                        ManagerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                        Jobs = jobs
                    });
                else
                    return View("Error");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreatePersonelManager()
        {

            HttpResponseMessage response = await _httpClient.GetAsync($"api/Job/GetAllJobs/");
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                dynamic jobList = JsonConvert.DeserializeObject(apiResponse);

                foreach (var job in jobList)
                {
                    jobs.Add(new JobVM()
                    {
                        Id = job.id,
                        Title = job.title,
                        Description = job.description
                    });
                }

                HttpResponseMessage message = await _httpClient.GetAsync($"api/Department/GetDepartmentsByCompanyId/{Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value)}");

                var apiResponse2 = await message.Content.ReadAsStringAsync();
                dynamic departmentList = JsonConvert.DeserializeObject(apiResponse2);

                List<DepartmentVM> departments = new List<DepartmentVM>();

                foreach (var department in departmentList)
                {
                    departments.Add(new DepartmentVM()
                    {
                        Id = department.id,
                        DepartmentName = department.departmentName,
                        Description = department.description
                    });
                }

                return View(new CreateEmployeeDTO()
                {
                    Departments = departments,
                    Jobs = jobs,
                    ManagerId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                });
            }
            else
            {
                return View("Error");
            }

        }


        [HttpPost]
        public async Task<IActionResult> CreatePersonel(CreateEmployeeDTO model)
        {
            if (User.IsInRole("CompanyManager"))
            {
                HttpResponseMessage message = await _httpClient.GetAsync($"api/AppUser/GetManagerByDepartmentId/{model.DepartmentId}");
                if (message.IsSuccessStatusCode)
                {
                    var apiResponse = await message.Content.ReadAsStringAsync();
                    dynamic manager = JsonConvert.DeserializeObject(apiResponse);
                    model.ManagerId = manager.id;
                }
                else
                {
                    return View("Error");
                }
            }
            model.ImagePath = model.Gender == Domain.Enum.Gender.Female ? "https://ik.imagekit.io/7ypp4olwr/femaledefault.png?tr=h-200,w-200" : "https://ik.imagekit.io/7ypp4olwr/maledefault.png?tr=h-200,w-200";

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/AppUser", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees");
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonelManager(CreateEmployeeDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.ImagePath = model.Gender == Domain.Enum.Gender.Female ? "https://ik.imagekit.io/7ypp4olwr/femaledefault.png?tr=h-200,w-200" : "https://ik.imagekit.io/7ypp4olwr/maledefault.png?tr=h-200,w-200";

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/AppUser/CreatePersonelManager", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Employees");
            }
            else
            {
                ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
                return View(model);
            }



        }

        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/AppUser/{id}");

            if (response.IsSuccessStatusCode)
            {
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
                foreach (var job in jobs)
                {
                    jobs.Add(new JobVM()
                    {
                        Id = job.Id,
                        Title = job.Title,
                        Description = job.Description
                    });
                }
                model.Jobs = jobs;
                return View(model);
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(UpdateUserDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Result"] = "modelinvalid";
                return View(model);
            }

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/AppUser", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Employees));
            }

            ModelState.AddModelError(response.StatusCode.ToString(), "Bir hata oluştu.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Home()
        {
            //var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            //var companyId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "CompanyId").Value);

            //var responseLeave = await _httpClient.GetAsync($"api/Leave/FillDashboardLeaveVM/{userId}");
            //var responseAdvance = await _httpClient.GetAsync($"api/Advance/FillDashboardAdvanceVM/{userId}");
            //var responseExpense = await _httpClient.GetAsync($"api/Expense/FillDashboardExpenseVM/{userId}");
            //var responseCompany = await _httpClient.GetAsync($"api/Company/GetCompanyVM/{companyId}");

            //if (responseLeave.IsSuccessStatusCode && responseAdvance.IsSuccessStatusCode && responseExpense.IsSuccessStatusCode && responseCompany.IsSuccessStatusCode)
            //{
            //    var contentLeave = await responseLeave.Content.ReadAsStringAsync();
            //    var dashboardLeaveVM = JsonConvert.DeserializeObject<List<DashboardLeaveVM>>(contentLeave);

            //    var contentAdvance = await responseAdvance.Content.ReadAsStringAsync();
            //    var dashboardAdvanceVM = JsonConvert.DeserializeObject<List<DashboardAdvanceVM>>(contentAdvance);

            //    var contentExpense = await responseExpense.Content.ReadAsStringAsync();
            //    var dashboardExpenseVM = JsonConvert.DeserializeObject<List<DashboardExpenseVM>>(contentExpense);

            //    var contentCompany = await responseCompany.Content.ReadAsStringAsync();
            //    var dashboardCompanyVM = JsonConvert.DeserializeObject<CompanyVM>(contentCompany);

            //    DashboardVM dashboardVM = new DashboardVM()
            //    {
            //        Leaves = dashboardLeaveVM,
            //        Advances = dashboardAdvanceVM,
            //        Expenses = dashboardExpenseVM,
            //        Company = dashboardCompanyVM
            //    };

            //    return View(dashboardVM);
            //}

            //return View("Error");
            return View();
        }


    }
}
