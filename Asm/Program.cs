using System;
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
            string program = @"#$ m. 05 &z &5";

            Lexer.Tokenize(program).ForEach(Console.WriteLine);
        }
    }
}
