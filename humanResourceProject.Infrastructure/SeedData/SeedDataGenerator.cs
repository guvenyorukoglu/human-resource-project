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


        public static async void Seed(IApplicationBuilder app, int maxCompanyCount)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                AppDbContext context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.Migrate();

                await CreateRolesAsync(serviceScope);
                await CreateSiteManagerAsync(serviceScope);

                if (!await context.Companies.AnyAsync())
                {
                    CreateFakeCompanies(maxCompanyCount);
                    context.Companies.AddRangeAsync(companies);
                }

                if (!await context.AppUsers.AnyAsync())
                {
                    CreateFakeEmployeesAsync();
                    context.AppUsers.AddRangeAsync(appUsers);
                }
                context.SaveChangesAsync();



                void CreateFakeCompanies(int maxCompanyCount)
                {
                    for (int i = 0; i < maxCompanyCount; i++)
                    {
                        var companyFake = new Faker<Company>()
                            .RuleFor(c => c.Id, f => f.Random.Guid())
                            .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                            .RuleFor(c => c.NumberOfEmployees, f => f.Random.Int(1, 20))
                            .RuleFor(c => c.TaxNumber, f => f.Random.ULong(1000000000, 9999999999).ToString())
                            .RuleFor(c => c.TaxOffice, f => f.Address.City())
                            .RuleFor(c => c.Address, f => f.Address.FullAddress())
                            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                            .RuleFor(c => c.CreateDate, f => f.Date.Past(1))
                            .RuleFor(c => c.Status, f => f.Random.Bool(0.9f) ? Status.Active : (f.Random.Bool(0.5f) ? Status.Modified : Status.Inactive));

                        companies.Add(companyFake.Generate());
                        foreach (var company in companies)
                        {
                            if (company.Status == Status.Inactive)
                            {
                                company.DeleteDate = GetRandomDayAfterCreateDate(company.CreateDate);
                            }
                            if (company.Status == Status.Modified)
                            {
                                company.UpdateDate = GetRandomDayAfterCreateDate(company.CreateDate); ;
                            }
                        }
                    }
                }

                async Task CreateFakeEmployeesAsync()
                {
                    foreach (var company in companies)
                    {
                        for (int i = 0; i < company.NumberOfEmployees; i++)
                        {
                            var employeeFake = new Faker<AppUser>()
                            .RuleFor(e => e.Id, f => f.Random.Guid())
                            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                            .RuleFor(e => e.LastName, f => f.Name.LastName())
                            .RuleFor(e => e.MiddleName, f => f.Random.Bool(0.2f) ? f.Name.FirstName() : null)
                            .RuleFor(e => e.Email, f => f.Person.Email)
                            .RuleFor(e => e.Address, f => f.Address.FullAddress())
                            .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
                            .RuleFor(e => e.IdentificationNumber, f => f.Random.ULong(10000000000, 99999999999).ToString())
                            .RuleFor(e => e.BloodGroup, f => f.PickRandom<BloodGroup>())
                            .RuleFor(e => e.Birthdate, f => f.Date.Past(50))
                            .RuleFor(e => e.Title, f => f.PickRandom<Title>())
                            .RuleFor(e => e.Job, f => f.Name.JobTitle())
                            .RuleFor(c => c.CreateDate, f => f.Date.Past(1))
                            .RuleFor(e => e.Status, f => f.Random.Bool(0.9f) ? Status.Active : (f.Random.Bool(0.5f) ? Status.Modified : Status.Inactive))
                            .RuleFor(e => e.CompanyId, company.Id);

                            appUsers.Add(employeeFake.Generate());
                        }

                        var employee = appUsers.Where(x => x.CompanyId == company.Id).First();
                        await CreateCompanyManager(serviceScope, employee);

                    }
                    foreach (var appUser in appUsers)
                    {
                        if (appUser.Status == Status.Inactive)
                        {
                            appUser.DeleteDate = GetRandomDayAfterCreateDate(appUser.CreateDate);
                        }
                        if (appUser.Status == Status.Modified)
                        {
                            appUser.UpdateDate = GetRandomDayAfterCreateDate(appUser.CreateDate);
                        }
                    }
                }

            }
        }

        private static async Task CreateCompanyManager(IServiceScope? serviceScope, AppUser employee)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            string password = "Cm123+";

            employee.Status = Status.Active;

            await userManager.CreateAsync(employee, password);
            await userManager.AddToRoleAsync(employee, "CompanyManager");
        }

        private static DateTime GetRandomDayAfterCreateDate(DateTime createdDate)
        {
            int range = (DateTime.Today - createdDate).Days;
            Random rnd = new Random();
            return createdDate.AddDays(rnd.Next(range));
        }

        private static async Task CreateSiteManagerAsync(IServiceScope serviceScope)
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            string email = "sitemanager@hra.com";
            string password = "Sm123+";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                AppUser siteManager = new AppUser()
                {
                    FirstName = "SiteManager",
                    LastName = "SiteManager",
                    Status = Status.Active,
                    UserName = "SiteManager",
                    Email = email
                };

                await userManager.CreateAsync(siteManager, password);
                await userManager.AddToRoleAsync(siteManager, "SiteManager");
            }
        }

        private static async Task CreateRolesAsync(IServiceScope serviceScope)
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var roles = new[] { "SiteManager", "CompanyManager", "Personel" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}
