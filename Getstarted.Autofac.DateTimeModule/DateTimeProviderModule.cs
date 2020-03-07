using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Getstarted.Autofac.DateTimeModule
{
    public class DateTimeProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SystemDateTimeProvider>()
                .As<IDateTimeProvider>();
            builder.Register(c => new FixedDateTimeProvider(DateTime.MinValue))
                .As<IDateTimeProvider>();
        }
    }
}
