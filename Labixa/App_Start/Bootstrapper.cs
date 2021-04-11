using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Labixa.Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Outsourcing.Core.Authentication;
using Outsourcing.Data;
using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Models;
using Outsourcing.Data.Repository;
using Outsourcing.Service;
using SocialGoal.Mappings;

namespace Labixa
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }
        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();

            builder.RegisterType<SDKApiFundist>().As<ISDKApiFundist>().InstancePerHttpRequest();
            builder.RegisterType<SDKApiAdmin>().As<ISDKApiAdmin>().InstancePerHttpRequest();
            builder.RegisterType<TwoFactAuthAdmin>().As<ITwoFactAuthAdmin>().InstancePerHttpRequest();
            builder.RegisterType<ConfirmEmail>().As<IConfirmEmail>().InstancePerHttpRequest();
            builder.RegisterType<Captcha>().As<IValidateCapcha>().InstancePerHttpRequest();


            builder.RegisterAssemblyTypes(typeof(BlogRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(BlogService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(SDKApiFundist).Assembly)
          .Where(t => t.Name.EndsWith("Service"))
          .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(SDKApiAdmin).Assembly)
          .Where(t => t.Name.EndsWith("Service"))
          .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(TwoFactAuthAdmin).Assembly)
         .Where(t => t.Name.EndsWith("Service"))
         .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(ConfirmEmail).Assembly)
         .Where(t => t.Name.EndsWith("Service"))
         .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(DefaultFormsAuthentication).Assembly)
       .Where(t => t.Name.EndsWith("Authentication"))
       .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof(Captcha).Assembly)
       .Where(t => t.Name.EndsWith("ValidateCapcha"))
       .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.Register(c => new UserManager<User>(new UserStore<User>(new OutsourcingEntities())))
                .As<UserManager<User>>().InstancePerHttpRequest();
            builder.Register(c => new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new OutsourcingEntities())))
               .As<RoleManager<IdentityRole>>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof(SDKApiFundist).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces()
           .InstancePerRequest();
            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}