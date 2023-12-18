using Bogus;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.Enum;
using humanResourceProject.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace humanResourceProject.Infrastructure.SeedData
{
    public static class SeedDataGenerator
    {
        static List<AppUser> appUsers = new();
        static List<Company> companies = new();
        static List<Department> departments = new();
        static List<Job> jobs = new();
        static List<Advance> advances = new();
        static List<Expense> expenses = new();
        static List<Leave> leaves = new();
        static List<AppUser> companyManagers = new();
        static List<AppUser> departmentManagers = new();
        static string[] femaleDefaultImages = new string[]
        {
            "https://ik.imagekit.io/7ypp4olwr/female1.jpg?tr=h-200,w-200",
            "https://ik.imagekit.io/7ypp4olwr/female2.jpg?tr=h-200,w-200",
            "https://ik.imagekit.io/7ypp4olwr/female3.jpg?tr=h-200,w-200"
        };
        static string[] maleDefaultImages = new string[]
        {
            "https://ik.imagekit.io/7ypp4olwr/male1.jpg?tr=h-200,w-200",
            "https://ik.imagekit.io/7ypp4olwr/male2.jpg?tr=h-200,w-200",
            "https://ik.imagekit.io/7ypp4olwr/male3.jpg?tr=h-200,w-200",
            "https://ik.imagekit.io/7ypp4olwr/male4.jpg?tr=h-200,w-200"
        };
        static string[] receiptDefaultImages = new string[]
        {
            "https://ik.imagekit.io/7ypp4olwr/receipt1.png",
            "https://ik.imagekit.io/7ypp4olwr/receipt2.png",
            "https://ik.imagekit.io/7ypp4olwr/receipt3.jpg",
            "https://ik.imagekit.io/7ypp4olwr/receipt4.png",
            "https://ik.imagekit.io/7ypp4olwr/receipt5.png"
        };


        public static async void Seed(IApplicationBuilder app, int maxCompanyCount, int maxDepartmentCountPerCompany, int maxNumberOfEmployeesCountPerDepartment, int maxJobCount, int maxAdvanceCountPerEmployee, int maxExpenseCountPerEmployee, int maxLeaveCountPerEmployee)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                AppDbContext context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                context.Database.Migrate();
                await CreateRolesAsync(serviceScope);


                if (!await context.Companies.AnyAsync())
                {
                    CreateFakeCompanies(maxCompanyCount);
                    await context.Companies.AddRangeAsync(companies);
                }

                if (!await context.Departments.AnyAsync())
                {
                    await CreateFakeDepartmentsAsync(maxDepartmentCountPerCompany);
                    await context.Departments.AddRangeAsync(departments);
                }

                if (!await context.Jobs.AnyAsync())
                {
                    await CreateJobsAsync(maxJobCount);
                    await context.Jobs.AddRangeAsync(jobs);
                }

                if (!await context.AppUsers.AnyAsync())
                {
                    await CreateCompanyManagerAsync(serviceScope);
                    await CreateDepartmentManagerAsync(serviceScope);
                    await CreateFakeEmployeesAsync(maxNumberOfEmployeesCountPerDepartment);
                }

                await UpdateCompanyEmployeeCount();

                await context.SaveChangesAsync();

                await CreateRequestsAsync(maxAdvanceCountPerEmployee, maxExpenseCountPerEmployee, maxLeaveCountPerEmployee);
                await CreateSiteManagerAsync(serviceScope);

                void CreateFakeCompanies(int maxCompanyCount)
                {
                    for (int i = 0; i < maxCompanyCount; i++)
                    {
                        var companyFake = new Faker<Company>()
                            .RuleFor(c => c.Id, f => f.Random.Guid())
                            .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                            .RuleFor(c => c.NumberOfEmployees, 0)
                            .RuleFor(c => c.TaxNumber, f => f.Random.ULong(1000000000, 9999999999).ToString())
                            .RuleFor(c => c.TaxOffice, f => f.Address.City())
                            .RuleFor(c => c.Address, f => f.Address.FullAddress())
                            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("+90##########"))
                            .RuleFor(c => c.CreateDate, f => f.Date.Past(1))
                            .RuleFor(c => c.Status, f => f.Random.Bool(0.9f) ? Status.Active : (f.Random.Bool(0.5f) ? Status.Modified : (f.Random.Bool(0.25f) ? Status.Inactive : Status.Deleted)));

                        companies.Add(companyFake.Generate());
                    }
                    foreach (var company in companies)
                    {
                        if (company.Status == Status.Deleted)
                        {
                            company.DeleteDate = GetRandomDayAfterCreateDate(company.CreateDate);
                        }
                        if (company.Status == Status.Modified)
                        {
                            company.UpdateDate = GetRandomDayAfterCreateDate(company.CreateDate);
                        }
                    }
                }

                async Task CreateFakeDepartmentsAsync(int maxDepartmentCountPerCompany)
                {
                    foreach (var company in companies)
                    {
                        for (int i = 0; i < maxDepartmentCountPerCompany; i++)
                        {
                            var departmentFake = new Faker<Department>()
                            .RuleFor(d => d.Id, f => f.Random.Guid())
                            .RuleFor(d => d.DepartmentName, f => f.Commerce.Department())
                            .RuleFor(d => d.Description, f => f.Lorem.Sentence())
                            .RuleFor(d => d.CreateDate, f => f.Date.Past(1))
                            .RuleFor(d => d.Status, f => f.Random.Bool(0.9f) ? Status.Active : (f.Random.Bool(0.5f) ? Status.Modified : (f.Random.Bool(0.25f) ? Status.Inactive : Status.Deleted)))
                            .RuleFor(d => d.CompanyId, company.Id);

                            departments.Add(departmentFake.Generate());
                        }

                        var departmentFakeForCompanyManager = new Faker<Department>()
                            .RuleFor(d => d.Id, f => f.Random.Guid())
                            .RuleFor(d => d.DepartmentName, "President & Managing Director")
                            .RuleFor(d => d.Description, f => f.Lorem.Sentence())
                            .RuleFor(d => d.CreateDate, f => f.Date.Past(1))
                            .RuleFor(d => d.Status, f => Status.Active)
                            .RuleFor(d => d.CompanyId, company.Id);

                        departments.Add(departmentFakeForCompanyManager.Generate());

                    }
                    foreach (var department in departments)
                    {
                        if (department.Status == Status.Deleted)
                        {
                            department.DeleteDate = GetRandomDayAfterCreateDate(department.CreateDate);
                        }
                        if (department.Status == Status.Modified)
                        {
                            department.UpdateDate = GetRandomDayAfterCreateDate(department.CreateDate);
                        }
                    }
                }

                async Task CreateJobsAsync(int maxJobCount)
                {
                    for (int i = 0; i < maxJobCount; i++)
                    {
                        var jobFake = new Faker<Job>()
                        .RuleFor(j => j.Id, f => f.Random.Guid())
                        .RuleFor(j => j.Title, f => f.Name.JobTitle())
                        .RuleFor(j => j.Description, f => f.Lorem.Sentence())
                        .RuleFor(j => j.CreateDate, f => f.Date.Past(1))
                        .RuleFor(j => j.Status, f => f.Random.Bool(0.9f) ? Status.Active : (f.Random.Bool(0.5f) ? Status.Modified : (f.Random.Bool(0.25f) ? Status.Inactive : Status.Deleted)));

                        jobs.Add(jobFake.Generate());
                    }

                    var jobFakeForAdmin = new Faker<Job>()
                        .RuleFor(j => j.Id, f => f.Random.Guid())
                        .RuleFor(j => j.Title, f => "Administrator")
                        .RuleFor(j => j.Description, f => "A person responsible for carrying out the administration of a business or organization.")
                        .RuleFor(j => j.CreateDate, f => f.Date.Past(1))
                        .RuleFor(j => j.Status, f => Status.Active);

                    jobs.Add(jobFakeForAdmin.Generate());

                    foreach (var job in jobs)
                    {
                        if (job.Status == Status.Deleted)
                        {
                            job.DeleteDate = GetRandomDayAfterCreateDate(job.CreateDate);
                        }
                        if (job.Status == Status.Modified)
                        {
                            job.UpdateDate = GetRandomDayAfterCreateDate(job.CreateDate);
                        }
                    }
                }

                async Task CreateFakeEmployeesAsync(int maxNumberOfEmployeesCountPerDepartment)
                {
                    for (int i = 0; i < departments.Count; i++)
                    {
                        Guid departmentManagerId = departmentManagers.SingleOrDefault(x => x.DepartmentId == departments[i].Id).Id;
                        Random rnd = new Random();


                        for (int j = 0; j < rnd.Next(1, maxNumberOfEmployeesCountPerDepartment); j++)
                        {
                            Guid randomJobId = jobs[rnd.Next(0, jobs.Count)].Id;

                            var employeeFake = new Faker<AppUser>()
                            .RuleFor(e => e.Id, f => f.Random.Guid())
                            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                            .RuleFor(e => e.LastName, f => f.Name.LastName())
                            .RuleFor(e => e.MiddleName, f => f.Random.Bool(0.2f) ? f.Name.FirstName() : null)
                            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName).ToLowerInvariant())
                            .RuleFor(e => e.NormalizedEmail, (f, e) => e.Email.ToUpperInvariant())
                            .RuleFor(e => e.EmailConfirmed, _ => true)
                            .RuleFor(e => e.UserName, (f, e) => e.Email)
                            .RuleFor(e => e.NormalizedUserName, (f, e) => e.UserName.ToUpperInvariant())
                            .RuleFor(e => e.Address, f => f.Address.FullAddress())
                            .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber("+90##########"))
                            .RuleFor(e => e.IdentificationNumber, f => f.Random.ULong(10000000000, 99999999999).ToString())
                            .RuleFor(e => e.BloodGroup, f => f.PickRandom<BloodGroup>())
                            .RuleFor(e => e.Birthdate, f => f.Date.Past(50))
                            .RuleFor(e => e.Gender, f => f.PickRandom<Gender>())
                            .RuleFor(c => c.CreateDate, f => f.Date.Past(1))
                            .RuleFor(e => e.Status, f => f.Random.Bool(0.9f) ? Status.Active : (f.Random.Bool(0.5f) ? Status.Modified : (f.Random.Bool(0.25f) ? Status.Inactive : Status.Deleted)))
                            .RuleFor(e => e.ImagePath, f => f.PickRandom(femaleDefaultImages))
                            .RuleFor(e => e.ManagerId, departmentManagerId)
                            .RuleFor(e => e.JobId, randomJobId)
                            .RuleFor(e => e.DepartmentId, departments[i].Id);

                            appUsers.Add(employeeFake.Generate());
                        }

                    }
                    foreach (var appUser in appUsers)
                    {
                        if (appUser.Status == Status.Deleted)
                        {
                            appUser.DeleteDate = GetRandomDayAfterCreateDate(appUser.CreateDate);
                        }
                        if (appUser.Status == Status.Modified)
                        {
                            appUser.UpdateDate = GetRandomDayAfterCreateDate(appUser.CreateDate);
                        }
                        if (appUser.Gender == Gender.Male)
                        {
                            appUser.ImagePath = GetRandomImageFromArray(maleDefaultImages);
                        }

                        string password = "Pr123+";
                        await userManager.CreateAsync(appUser, password);
                        await userManager.AddToRoleAsync(appUser, "Personel");

                    }
                }

                async Task CreateRequestsAsync(int maxAdvanceCountPerEmployee, int maxExpenseCountPerEmployee, int maxLeaveCountPerEmployee)
                {
                    foreach (var employee in appUsers)
                    {
                        for (int i = 0; i < maxAdvanceCountPerEmployee; i++)
                        {
                            var advanceFake = new Faker<Advance>()
                            .RuleFor(a => a.Id, f => f.Random.Guid())
                            .RuleFor(a => a.AmountOfAdvance, f => f.Random.Decimal(100, 99999))
                            .RuleFor(a => a.Explanation, f => f.Lorem.Sentence())
                            .RuleFor(a => a.AdvanceType, f => f.PickRandom<AdvanceType>())
                            .RuleFor(a => a.Currency, f => f.PickRandom<Currency>())
                            .RuleFor(a => a.AdvanceStatus, f => f.Random.Bool(0.5f) ? RequestStatus.Approved : (f.Random.Bool(0.25f) ? RequestStatus.Pending : RequestStatus.Rejected))
                            .RuleFor(a => a.CreateDate, f => f.Date.Past(1))
                            .RuleFor(a => a.Status, f => Status.Active)
                            .RuleFor(a => a.EmployeeId, employee.Id);

                            advances.Add(advanceFake.Generate());
                        }

                        for (int i = 0; i < maxExpenseCountPerEmployee; i++)
                        {
                            var expenseFake = new Faker<Expense>()
                            .RuleFor(e => e.Id, f => f.Random.Guid())
                            .RuleFor(e => e.AmountOfExpense, f => f.Random.Decimal(100, 99999))
                            .RuleFor(e => e.DateOfExpense, f => f.Date.Past(1))
                            .RuleFor(e => e.ExpenseType, f => f.PickRandom<ExpenseType>())
                            .RuleFor(e => e.Explanation, f => f.Lorem.Sentence())
                            .RuleFor(e => e.Currency, f => f.PickRandom<Currency>())
                            .RuleFor(e => e.FilePath, f => f.PickRandom(receiptDefaultImages))
                            .RuleFor(e => e.ExpenseStatus, f => f.Random.Bool(0.5f) ? RequestStatus.Approved : (f.Random.Bool(0.25f) ? RequestStatus.Pending : RequestStatus.Rejected))
                            .RuleFor(e => e.CreateDate, f => f.Date.Past(1))
                            .RuleFor(e => e.Status, f => Status.Active)
                            .RuleFor(e => e.EmployeeId, employee.Id);

                            expenses.Add(expenseFake.Generate());
                        }

                        for (int i = 0; i < maxLeaveCountPerEmployee; i++)
                        {
                            int numberOfDays = new Random().Next(1, 30);
                            var leaveFake = new Faker<Leave>()
                            .RuleFor(l => l.Id, f => f.Random.Guid())
                            .RuleFor(l => l.StartDateOfLeave, f => f.Date.Past(1))
                            .RuleFor(l => l.EndDateOfLeave, (f, l) => f.Date.Soon(numberOfDays, l.StartDateOfLeave))
                            .RuleFor(l => l.LeaveType, f => f.PickRandom<LeaveType>())
                            .RuleFor(l => l.Explanation, f => f.Lorem.Sentence())
                            .RuleFor(l => l.DaysOfLeave, f => f.Random.Int(1, numberOfDays))
                            .RuleFor(l => l.LeaveStatus, f => f.Random.Bool(0.5f) ? RequestStatus.Approved : (f.Random.Bool(0.25f) ? RequestStatus.Pending : RequestStatus.Rejected))
                            .RuleFor(l => l.CreateDate, (f, l) => f.Date.Recent(numberOfDays, l.StartDateOfLeave))
                            .RuleFor(l => l.Status, f => Status.Active)
                            .RuleFor(l => l.EmployeeId, employee.Id);

                            leaves.Add(leaveFake.Generate());
                        }
                    }
                }
            }
        }

        private static async Task UpdateCompanyEmployeeCount()
        {
            companies.ForEach(c => c.NumberOfEmployees = appUsers.Count(x => x.Department.CompanyId == c.Id) + companyManagers.Count(x => x.Department.CompanyId == c.Id) + companyManagers.Count(x => x.Department.CompanyId == c.Id));
        }

        private static async Task CreateCompanyManagerAsync(IServiceScope? serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            foreach (var company in companies)
            {
                Random random = new Random();
                Guid randomJobId = jobs[random.Next(0, jobs.Count)].Id;

                var departmentsOfTheCompany = departments.Where(x => x.CompanyId == company.Id).ToList();
                Guid companyManagerDepartmentId = departments.SingleOrDefault(x => x.DepartmentName == "President & Managing Director" && x.CompanyId == company.Id).Id;

                var companyManagerFake = new Faker<AppUser>()
                        .RuleFor(e => e.Id, f => f.Random.Guid())
                        .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                        .RuleFor(e => e.LastName, f => f.Name.LastName())
                        .RuleFor(e => e.MiddleName, f => f.Random.Bool(0.2f) ? f.Name.FirstName() : null)
                        .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName).ToLowerInvariant())
                        .RuleFor(e => e.NormalizedEmail, (f, e) => e.Email.ToUpperInvariant())
                        .RuleFor(e => e.EmailConfirmed, _ => true)
                        .RuleFor(e => e.UserName, (f, e) => e.Email)
                        .RuleFor(e => e.NormalizedUserName, (f, e) => e.UserName.ToUpperInvariant())
                        .RuleFor(e => e.Address, f => f.Address.FullAddress())
                        .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber("+90##########"))
                        .RuleFor(e => e.IdentificationNumber, f => f.Random.ULong(10000000000, 99999999999).ToString())
                        .RuleFor(e => e.BloodGroup, f => f.PickRandom<BloodGroup>())
                        .RuleFor(e => e.Birthdate, f => f.Date.Past(50))
                        .RuleFor(e => e.Gender, Gender.Male)
                        .RuleFor(c => c.CreateDate, f => f.Date.Past(1))
                        .RuleFor(e => e.Status, _ => Status.Active)
                        .RuleFor(e => e.ImagePath, "https://ik.imagekit.io/7ypp4olwr/companymanager.png?tr=h-200,w-200")
                        .RuleFor(e => e.JobId, randomJobId)
                        .RuleFor(e => e.DepartmentId, companyManagerDepartmentId);

                AppUser companyManagerUser = companyManagerFake.Generate();
                companyManagers.Add(companyManagerUser);
                string password = "Cm123+";

                await userManager.CreateAsync(companyManagerUser, password);
                await userManager.AddToRoleAsync(companyManagerUser, "CompanyManager");

            }
        }

        private static async Task CreateDepartmentManagerAsync(IServiceScope? serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            for (int i = 0; i < companies.Count; i++)
            {
                var departmentsOfTheCompany = departments.Where(x => x.CompanyId == companies[i].Id);
                foreach (var department in departmentsOfTheCompany)
                {
                    Random random = new Random();
                    Guid randomJobId = jobs[random.Next(0, jobs.Count)].Id;

                    var departmentManagerFake = new Faker<AppUser>()
                        .RuleFor(e => e.Id, f => f.Random.Guid())
                        .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                        .RuleFor(e => e.LastName, f => f.Name.LastName())
                        .RuleFor(e => e.MiddleName, f => f.Random.Bool(0.2f) ? f.Name.FirstName() : null)
                        .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName).ToLowerInvariant())
                        .RuleFor(e => e.NormalizedEmail, (f, e) => e.Email.ToUpperInvariant())
                        .RuleFor(e => e.EmailConfirmed, _ => true)
                        .RuleFor(e => e.UserName, (f, e) => e.Email)
                        .RuleFor(e => e.NormalizedUserName, (f, e) => e.UserName.ToUpperInvariant())
                        .RuleFor(e => e.Address, f => f.Address.FullAddress())
                        .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber("+90##########"))
                        .RuleFor(e => e.IdentificationNumber, f => f.Random.ULong(10000000000, 99999999999).ToString())
                        .RuleFor(e => e.BloodGroup, f => f.PickRandom<BloodGroup>())
                        .RuleFor(e => e.Birthdate, f => f.Date.Past(50))
                        .RuleFor(e => e.Gender, Gender.Female)
                        .RuleFor(c => c.CreateDate, f => f.Date.Past(1))
                        .RuleFor(e => e.Status, _ => Status.Active)
                        .RuleFor(e => e.ImagePath, "https://ik.imagekit.io/7ypp4olwr/departmentmanager.png?tr=h-200,w-200")
                        .RuleFor(e => e.ManagerId, companyManagers[i].Id)
                        .RuleFor(e => e.JobId, randomJobId)
                        .RuleFor(e => e.DepartmentId, department.Id);

                    AppUser departmentManagerUser = departmentManagerFake.Generate();
                    departmentManagers.Add(departmentManagerUser);
                    string password = "Dm123+";

                    await userManager.CreateAsync(departmentManagerUser, password);
                    await userManager.AddToRoleAsync(departmentManagerUser, "DepartmentManager");
                }
            }
        }

        private static DateTime GetRandomDayAfterCreateDate(DateTime createdDate)
        {
            int range = (DateTime.Today - createdDate).Days;
            Random rnd = new Random();
            return createdDate.AddDays(rnd.Next(range));
        }
        private static string? GetRandomImageFromArray(string[] maleDefaultImages)
        {
            Random random = new Random();
            return maleDefaultImages[random.Next(0, maleDefaultImages.Length)];
        }

        private static async Task CreateSiteManagerAsync(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var _context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            string email = "sitemanager@monitorease.com";
            string password = "Sm123+";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                Company company = new Company()
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "MonitorEase",
                    NumberOfEmployees = 1,
                    TaxNumber = "1234567890",
                    TaxOffice = "İstanbul",
                    Address = "İstanbul",
                    PhoneNumber = "+901234567890",
                    CreateDate = DateTime.Now,
                    Status = Status.Active
                };
                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();

                Department department = new Department()
                {
                    Id = Guid.NewGuid(),
                    DepartmentName = "Information Technology Department",
                    Description = "Information technology management or IT management is the discipline whereby all of the information technology resources of a firm are managed in accordance with its needs and priorities.",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    CompanyId = company.Id
                };

                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();

                AppUser siteManager = new AppUser()
                {
                    FirstName = "Site",
                    LastName = "Manager",
                    IdentificationNumber = "12345678901",
                    BloodGroup = BloodGroup.OPositive,
                    Birthdate = DateTime.Now,
                    Gender = Gender.Male,
                    JobId = jobs.SingleOrDefault(x => x.Title == "Administrator").Id,
                    Address = "İstanbul",
                    PhoneNumber = "901234567890",
                    CreateDate = DateTime.Now,
                    Status = Status.Active,
                    UserName = "SiteManager",
                    ImagePath = "https://ik.imagekit.io/7ypp4olwr/admin.jpeg?tr=h-200,w-200",
                    Email = email,
                    DepartmentId = department.Id,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(siteManager, password);
                await userManager.AddToRoleAsync(siteManager, "SiteManager");
            }
        }

        private static async Task CreateRolesAsync(IServiceScope serviceScope)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var roles = new[] { "SiteManager", "CompanyManager", "DepartmentManager", "Personel" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}
