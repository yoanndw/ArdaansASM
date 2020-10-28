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
            string program = @"inc  ";

            var input = new Input(program);

            var tokens = Lexer.Tokenize(input);

            try
            {
                var ast = Parser.Parse(input, tokens);
                ast.ForEach(Console.WriteLine);
            } 
            catch (ParseErrorsException e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
