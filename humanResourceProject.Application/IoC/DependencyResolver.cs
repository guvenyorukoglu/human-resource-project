﻿using Autofac;
using AutoMapper;
using humanResourceProject.Application.AutoMapper;
using humanResourceProject.Application.Services.Abstract.IAppUserServices;
using humanResourceProject.Application.Services.Abstract.ICompanyServices;
using humanResourceProject.Application.Services.Concrete.AppUserServices;
using humanResourceProject.Application.Services.Concrete.CompanyServices;
using humanResourceProject.Domain.IRepository.AppUserRepo;
using humanResourceProject.Domain.IRepository.CompanyRepo;
using humanResourceProject.Infrastructure.Repositories.AppUserRepos;
using humanResourceProject.Infrastructure.Repositories.CompanyRepos;

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

            //Repositories Absrtact to Concrete
            builder.RegisterType<AppUserReadRepository>().As<IAppUserReadRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AppUserWriteRepository>().As<IAppUserWriteRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyReadRepository>().As<ICompanyReadRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyWriteRepository>().As<ICompanyWriteRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();

            #region AutoMapper //Copy-Paste
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
            #endregion

            base.Load(builder);
        }
    }
}
