using System;
using System.Collections.Generic;
using System.Text;

namespace Ardaans.Assembly
{
    public class FailedAssemblingException : Exception
    {
        public FailedAssemblingException(Exception inner)
            : base("Failed to assemble.") { }
    }
}
