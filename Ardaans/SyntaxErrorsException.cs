using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Ardaans
{
    public class SyntaxErrorsException : Exception
    {
        public SyntaxErrorsException()
            : base("Syntax errors occured. Cannot continue assembling")
        {
        }
    }
}