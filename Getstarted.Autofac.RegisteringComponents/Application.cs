using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Getstarted.Autofac.DateTimeModule;

namespace Getstarted.Autofac.RegisteringComponents
{
    public class Application
    {
        static void Main(string[] args)
        {
            RegisterAndResolve();
            RegisterMultipleAndResolveWithDefault();
            RegisterMultiplePreserveExistingDefaultAndResolve();

            RegisterModule();

            RegisterAssembly();

            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }

        private static void RegisterAndResolve()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            builder.RegisterType<SystemDateTimeProvider>()
                .As<IDateTimeProvider>();
            Console.WriteLine("# Registered {0} as {1}.",
                typeof(SystemDateTimeProvider).Name,
                typeof(IDateTimeProvider).Name);

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved {0}#CurrentDateTime = {1}", 
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }
        }

        private static void RegisterMultipleAndResolveWithDefault()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            builder.RegisterType<SystemDateTimeProvider>()
                .As<IDateTimeProvider>();
            Console.WriteLine("# Registered 1- {0} as {1}.",
                typeof(SystemDateTimeProvider).Name,
                typeof(IDateTimeProvider).Name);

            builder.Register(c => new FixedDateTimeProvider(DateTime.MinValue))
                .As<IDateTimeProvider>();
            Console.WriteLine("# Registered 2- {0} as {1}.",
                typeof(FixedDateTimeProvider).Name,
                typeof(IDateTimeProvider).Name);

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved Last Registered {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }
        }


        private static void RegisterMultiplePreserveExistingDefaultAndResolve()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            builder.RegisterType<SystemDateTimeProvider>()
                .As<IDateTimeProvider>();
            Console.WriteLine("# Registered 1- {0} as {1}.",
                typeof(SystemDateTimeProvider).Name,
                typeof(IDateTimeProvider).Name);

            builder.Register(c => new FixedDateTimeProvider(DateTime.MinValue))
                .As<IDateTimeProvider>()
                .PreserveExistingDefaults();
            Console.WriteLine("# Registered 2- {0} as {1}, with PreservingExistingDefaults.",
                typeof(FixedDateTimeProvider).Name,
                typeof(IDateTimeProvider).Name);

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved Preserved Defaults {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }
        }

        private static void RegisterModule()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            builder.RegisterModule<DateTimeProviderModule>();
            Console.WriteLine("# Registered Module {0}.",
                typeof(DateTimeProviderModule).Name);
            
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved Module Registered {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }
        }

        private static void RegisterAssembly()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyModules<DateTimeProviderModule>(assemblies);
            Console.WriteLine("# Registered Module {0} from {1} assemblies.",
                typeof(DateTimeProviderModule).Name,
                assemblies.Count());

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved Module Registered {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }
        }
    }
}
