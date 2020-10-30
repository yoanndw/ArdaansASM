using System;
using System.Collections.Generic;
using System.Text;

namespace Asm.Assembly
{
    public class FailedAssemblingException : Exception
    {
        public FailedAssemblingException(Exception inner)
            : base("Failed to assemble.") { }
    }
}
