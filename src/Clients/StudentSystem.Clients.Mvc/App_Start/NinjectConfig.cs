using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using MediatR;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Modules;
using Ninject.Planning.Bindings;
using Ninject.Planning.Bindings.Resolvers;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using StudentSystem.Authentication;
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
                RegisterAuthenticationModule(kernel);
                RegisterMediatorModule(kernel);
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
            //kernel.Bind<IAccountService>().To<AccountService>().InRequestScope();
        }

        private static void RegisterAuthenticationModule(IKernel kernel)
        {
            kernel.Bind<StudentSystemAuthDbContext>().ToSelf().InRequestScope();

            kernel.Bind<IAuthenticationService>().To<AuthenticationService>().InRequestScope();
        }

        private static void RegisterMediatorModule(IKernel kernel)
        {
            kernel.BindMediatR();

            // That's it! Make sure to bind the handlers as needed.
          

            kernel.Bind<INotificationHandler<AccountCreated>>().To<StudentCreatedHandler>();
        }
    }

    // TODO copy/paste or nuget ?
    public class ContravariantBindingResolver : NinjectComponent, IBindingResolver
    {
        /// <summary>
        /// Returns any bindings from the specified collection that match the specified service.
        /// </summary>
        public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
        {
            if (service.IsGenericType)
            {
                var genericType = service.GetGenericTypeDefinition();
                var genericArguments = genericType.GetGenericArguments();
                var isContravariant = genericArguments.Length == 1
                                      && genericArguments
                                          .Single()
                                          .GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant);
                if (isContravariant)
                {
                    var argument = service.GetGenericArguments().Single();
                    var matches = bindings.Where(kvp => kvp.Key.IsGenericType
                                                        && kvp.Key.GetGenericTypeDefinition() == genericType
                                                        && kvp.Key.GetGenericArguments().Single() != argument
                                                        && kvp.Key.GetGenericArguments().Single().IsAssignableFrom(argument))
                        .SelectMany(kvp => kvp.Value);
                    return matches;
                }
            }

            return Enumerable.Empty<IBinding>();
        }
    }

    public class MediatRModule : NinjectModule
    {
        public override void Load()
        {
            Kernel?.Components.Add<IBindingResolver, ContravariantBindingResolver>();

            Kernel?.Bind<IMediator>().To<Mediator>();
            Kernel?.Bind<ServiceFactory>().ToMethod(ctx => t => ctx.Kernel.TryGet(t));
        }
    }

    public static class NinjectKernelExtensions
    {
        /// <summary>
        /// Loads the MediaR Ninject Module in the given kernel.
        /// <see cref="MediatRModule"/>
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public static IKernel BindMediatR(this IKernel kernel)
        {
            kernel.Load<MediatRModule>();
            return kernel;
        }
    }
}
