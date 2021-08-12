using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okqwii.Common.ExceptionHandler.Exceptions
{
    public class InternalException : Exception
    {
        public InternalException() { }

        public InternalException(string message)
            : base(message) { }

        public InternalException(string message, Exception inner)
            : base(message, inner) { }
    }
}
