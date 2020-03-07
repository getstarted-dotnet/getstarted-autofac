using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getstarted.Autofac.DateTimeModule
{
    public interface IDateTimeProvider
    {
        DateTime CurrentDateTime { get; }
    }
}
