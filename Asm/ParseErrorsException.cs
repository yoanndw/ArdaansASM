using System;
using System.Collections.Generic;
using System.Text;

namespace Asm
{
    public class ParseErrorsException : Exception
    {
        public ParseErrorsException()
            : base("Parse errors occured. Cannot continue assembling") { }
    }
}
