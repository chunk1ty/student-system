using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;

using StudentSystem.Clients.Mvc;
using StudentSystem.Clients.Mvc.Infrastructure;
using StudentSystem.Persistence;
using StudentSystem.Persistence.Contracts;
using StudentSystem.Persistence.Repositories;
using StudentSystem.Domain.Services;
using StudentSystem.Domain.Services.Contracts;
using StudentSystem.Infrastructure.Mapping;
using StudentSystem.Infrastructure.RetryPolicy;
using StudentSystem.Infrastructure.Security;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(NinjectConfig), "Stop")]

namespace StudentSystem.Clients.Mvc
{
    public static class NinjectConfig
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterMvcModule(kernel);
                RegisterDataModule(kernel);
                RegisterInfrastructureModule(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
       
        private static void RegisterMvcModule(IKernel kernel)
        {
            kernel.Bind<FormsAuthenticationWrapper>().ToSelf().InRequestScope();
        }

        private static void RegisterInfrastructureModule(IKernel kernel)
        {
            kernel.Bind<IMappingService>().To<MappingService>().InSingletonScope();
            kernel.Bind<ICypher>().To<RijndaelPbkdf2Cyhper>().InSingletonScope();
            kernel.Bind<IRetryPolicy>().To<RetryPolicyService>();
        }

        // TODO DI modules ?
        private static void RegisterDataModule(IKernel kernel)
        {
            kernel.Bind<StudentSystemDbContext>().ToSelf().InRequestScope();

            kernel.Bind(typeof(IUnitOfWork))
                  .ToMethod(ctx => ctx.Kernel.Get<StudentSystemDbContext>())
                  .InRequestScope();

            kernel.Bind<IStudentRepository>().To<EfStudentRepository>().InRequestScope();
            kernel.Bind<ICourseRepository>().To<EfCourseRepository>().InRequestScope();

            kernel.Bind<ICourseService>().To<CourseService>().InRequestScope();
            kernel.Bind<IStudentService>().To<StudentService>().InRequestScope();
            kernel.Bind<IAccountService>().To<AccountService>().InRequestScope();
        }
    }
}
