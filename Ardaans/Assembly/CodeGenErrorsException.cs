﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ardaans.Assembly
{
    public class CodeGenErrorsException : Exception
    {
        public CodeGenErrorsException()
            : base("Parse errors occured. Cannot continue assembling") { }
    }
}
