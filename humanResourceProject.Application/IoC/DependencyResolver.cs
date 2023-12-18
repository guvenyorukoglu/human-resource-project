using Autofac;
using AutoMapper;
using humanResourceProject.Application.AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAdvanceServices;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Abstract.IDepartmantServices;
using humanResourceProject.Application.Services.Abstract.IImageServices;
using humanResourceProject.Application.Services.Abstract.IJobServices;
using humanResourceProject.Application.Services.Abstract.ILeaveServices;
using humanResourceProject.Application.Services.Abstract.IMailServices;
using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Application.Services.Concrete.AdvanceServices;
using humanResourceProject.Application.Services.Concrete.AppUserServices;
using humanResourceProject.Application.Services.Concrete.BaseServices;
using humanResourceProject.Application.Services.Concrete.CompanyServices;
using humanResourceProject.Application.Services.Concrete.DeparmentServices;
using humanResourceProject.Application.Services.Concrete.ImageServices;
using humanResourceProject.Application.Services.Concrete.JobServices;
using humanResourceProject.Application.Services.Concrete.LeaveServices;
using humanResourceProject.Application.Services.Concrete.MailServices;
using humanResourceProject.Domain.Entities.Concrete;


using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Domain.IRepository.ExpenseRepo;
using humanResourceProject.Infrastructure.Repositories.AdvanceRepos;
using humanResourceProject.Infrastructure.Repositories.AppUserRepos;
using humanResourceProject.Infrastructure.Repositories.CompanyRepos;
using humanResourceProject.Infrastructure.Repositories.DepartmentRepos;
using humanResourceProject.Infrastructure.Repositories.ExpenseRepo;
using humanResourceProject.Infrastructure.Repositories.JobRepos;
using humanResourceProject.Infrastructure.Repositories.LeaveRepos;

namespace humanResourceProject.Application.IoC
{
    public class DependencyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //Services Absrtact to Concrete
            builder.RegisterType<AppUserReadService>().As<IAppUserReadService>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserWriteService>().As<IAppUserWriteService>().InstancePerLifetimeScope();

            builder.RegisterType<CompanyReadService>().As<ICompanyReadService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyWriteService>().As<ICompanyWriteService>().InstancePerLifetimeScope();

            builder.RegisterType<ImageService>().As<IImageService>().InstancePerLifetimeScope();

            builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();

            builder.RegisterType<AdvanceReadService>().As<IAdvanceReadService>().InstancePerLifetimeScope();
            builder.RegisterType<AdvanceWriteService>().As<IAdvanceWriteService>().InstancePerLifetimeScope();

            builder.RegisterType<DepartmentReadService>().As<IDepartmentReadService>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentWriteService>().As<IDepartmentWriteService>().InstancePerLifetimeScope();
            
            builder.RegisterType<ExpenseReadRepository>().As<IExpenseReadRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ExpenseWriteRepository>().As<IExpenseWriteRepository>().InstancePerLifetimeScope();

            builder.RegisterType<JobReadService>().As<IJobReadService>().InstancePerLifetimeScope();
            builder.RegisterType<JobWriteService>().As<IJobWriteService>().InstancePerLifetimeScope();

            builder.RegisterType<LeaveReadService>().As<ILeaveReadService>().InstancePerLifetimeScope();
            builder.RegisterType<LeaveWriteService>().As<ILeaveWriteService>().InstancePerLifetimeScope();

            builder.RegisterType<BaseWriteService<AppUser>>().As<IBaseWriteService<AppUser>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseWriteService<Company>>().As<IBaseWriteService<Company>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseWriteService<Advance>>().As<IBaseWriteService<Advance>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseWriteService<Department>>().As<IBaseWriteService<Department>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseWriteService<Expense>>().As<IBaseWriteService<Expense>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseWriteService<Job>>().As<IBaseWriteService<Job>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseWriteService<Leave>>().As<IBaseWriteService<Leave>>().InstancePerLifetimeScope();

            builder.RegisterType<BaseReadService<AppUser>>().As<IBaseReadService<AppUser>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseReadService<Company>>().As<IBaseReadService<Company>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseReadService<Advance>>().As<IBaseReadService<Advance>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseReadService<Department>>().As<IBaseReadService<Department>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseReadService<Expense>>().As<IBaseReadService<Expense>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseReadService<Job>>().As<IBaseReadService<Job>>().InstancePerLifetimeScope();
            builder.RegisterType<BaseReadService<Leave>>().As<IBaseReadService<Leave>>().InstancePerLifetimeScope();



            //Repositories Absrtact to Concrete
            builder.RegisterType<AppUserReadRepository>().As<IBaseReadRepository<AppUser>>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserWriteRepository>().As<IBaseWriteRepository<AppUser>>().InstancePerLifetimeScope();

            builder.RegisterType<CompanyReadRepository>().As<IBaseReadRepository<Company>>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyWriteRepository>().As<IBaseWriteRepository<Company>>().InstancePerLifetimeScope();

            builder.RegisterType<AdvanceReadRepository>().As<IBaseReadRepository<Advance>>().InstancePerLifetimeScope();
            builder.RegisterType<AdvanceWriteRepository>().As<IBaseWriteRepository<Advance>>().InstancePerLifetimeScope();

            builder.RegisterType<DepartmentReadRepository>().As<IBaseReadRepository<Department>>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentWriteRepository>().As<IBaseWriteRepository<Department>>().InstancePerLifetimeScope();

            builder.RegisterType<ExpenseReadRepository>().As<IBaseReadRepository<Expense>>().InstancePerLifetimeScope();
            builder.RegisterType<ExpenseWriteRepository>().As<IBaseWriteRepository<Expense>>().InstancePerLifetimeScope();

            builder.RegisterType<JobReadRepository>().As<IBaseReadRepository<Job>>().InstancePerLifetimeScope();
            builder.RegisterType<JobWriteRepository>().As<IBaseWriteRepository<Job>>().InstancePerLifetimeScope();

            builder.RegisterType<LeaveReadRepository>().As<IBaseReadRepository<Leave>>().InstancePerLifetimeScope();
            builder.RegisterType<LeaveWriteRepository>().As<IBaseWriteRepository<Leave>>().InstancePerLifetimeScope();


            //Mapper
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                //Register Mapper Profile
                cfg.AddProfile<Mapping>(); /// AutoMapper klasörünün altına eklediğimiz Mapping classını bağlıyoruz.
            }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
