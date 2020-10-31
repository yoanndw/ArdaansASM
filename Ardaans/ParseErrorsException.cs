using System;
using System.Collections.Generic;
using System.Text;

namespace Ardaans
{
    public class ParseErrorsException : Exception
    {
        public ParseErrorsException()
            : base("Parse errors occured. Cannot continue assembling") { }
    }
}
