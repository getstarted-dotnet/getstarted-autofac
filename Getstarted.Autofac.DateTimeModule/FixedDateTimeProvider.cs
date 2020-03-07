using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Getstarted.Autofac.DateTimeModule
{
    public class FixedDateTimeProvider : IDateTimeProvider
    {
        private DateTime FixedDateTime { get; set; }

        public FixedDateTimeProvider(DateTime fixedDateTime)
        {
            FixedDateTime = fixedDateTime;
        }

        public DateTime CurrentDateTime => FixedDateTime;
    }
}
