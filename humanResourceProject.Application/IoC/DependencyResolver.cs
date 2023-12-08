using Autofac;
using AutoMapper;
using humanResourceProject.Application.AutoMapper;

namespace humanResourceProject.Application.IoC
{
    public class DependencyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Services Absrtact to Concrete

            //builder.RegisterType<PostServices>().As<IPostService>().InstancePerLifetimeScope();
            //builder.RegisterType<GenreService>().As<IGenreService>().InstancePerLifetimeScope();


            //Repositories Absrtact to Concrete

            //builder.RegisterType<PostRepository>().As<IPostRepository>().InstancePerLifetimeScope();
            //builder.RegisterType<GenreRepository>().As<IGenreRepository>().InstancePerLifetimeScope();
            //builder.RegisterType<AuthorRepository>().As<IAuthorRepository>().InstancePerLifetimeScope();

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
