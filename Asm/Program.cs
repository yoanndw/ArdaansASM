using System;
using System.IO;
using System.Collections.Generic;

using Asm.Assembly;
using Asm.Parsing;
using Asm.Tokens;

namespace Asm
{
    class Program
    {
        static void Main(string[] args)
        {
            string program = @"inc #$5 mov #$0A a";

            try
            {
                byte[] code = Assembler.Assemble(program);
            }
            catch (FailedAssemblingException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
