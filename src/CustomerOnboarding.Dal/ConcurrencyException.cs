using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Dal
{
    [Serializable]
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message)
          : base(message)
        { }

        public ConcurrencyException(string message, Exception innerException)
          : base(message, innerException)
        { }
    }
}
