using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Getstarted.Autofac.DateTimeModule;
using Microsoft.Extensions.Configuration;

namespace Getstarted.Autofac.RegisteringComponents
{
    public class Application
    {
        static void Main(string[] args)
        {
            RegisterSingle();
            RegisterMultiple();
            RegisterMultiplePreserveExistingDefault();

            RegisterModule();
            RegisterConfigurationModule();

            RegisterAssembly();

            RegisterAsInstanceLifetimeScope();
            RegisterAsSingleton();

            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }

        private static void RegisterSingle()
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

        private static void RegisterMultiple()
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


        private static void RegisterMultiplePreserveExistingDefault()
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

        private static void RegisterConfigurationModule()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            var config = new ConfigurationBuilder();
            config.AddJsonFile("Autofac.json");
            var configModule = new ConfigurationModule(config.Build());
            builder.RegisterModule(configModule);
            Console.WriteLine("# Registered Configuration Module from Autofac.json file.");

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

        private static void RegisterAsInstanceLifetimeScope()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            builder.Register(c => new FixedDateTimeProvider(DateTime.Now))
                .As<IDateTimeProvider>().InstancePerLifetimeScope();
            Console.WriteLine("# Registered {0} As InstancePerLifetimeScope.",
                typeof(FixedDateTimeProvider).Name);

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }

            Thread.Sleep(2000);
            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved in another scope after 2 sec {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }
        }


        private static void RegisterAsSingleton()
        {
            Console.WriteLine();
            var builder = new ContainerBuilder();

            builder.Register(c => new FixedDateTimeProvider(DateTime.Now))
                .As<IDateTimeProvider>().SingleInstance();
            Console.WriteLine("# Registered {0} As Singleton.",
                typeof(FixedDateTimeProvider).Name);

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }

            Thread.Sleep(2000);
            using (var scope = container.BeginLifetimeScope())
            {
                var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
                Console.WriteLine("Resolved in another scope after 2 sec {0}#CurrentDateTime = {1}",
                    dateTimeProvider.GetType().Name, dateTimeProvider.CurrentDateTime);
            }
        }

    }
}
