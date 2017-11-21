using MockTestExample.Website;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace MockTestExample.Website
{
    using System;
    using System.Web;

    using log4net;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using MockTestExample.DataAccess;
    using MockTestExample.DataAccess.Interfaces;

    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target.Member.ReflectedType));

            kernel.Bind<IDbContext>().To<ApplicationDbContext>().InRequestScope();

            kernel.Bind(x => x.FromAssembliesMatching("ABP.MockTestExample.DataAccess.dll")
                    .SelectAllClasses()

                    .InheritedFrom(typeof(IRepositoryBase<>))
                    .Where(w => w.IsPublic && w.IsClass)
                    .BindDefaultInterface()
                    .Configure(c => c.InRequestScope()));

            //Abp.Service.InjectMe.RegisterServices(kernel);
        }
    }
}
