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
            string program = @"mzv # $ 05 &$ &f #$555";

            Lexer.Tokenize(program).ForEach(Console.WriteLine);
        }
    }
}
