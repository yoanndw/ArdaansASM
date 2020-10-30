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

            var input = new Input(program);

            var tokens = Lexer.Tokenize(input);

            var ast = Parser.Parse(input, tokens);
            //ast.ForEach(Console.WriteLine);

            try
            {
                var code = CodeGenerator.GenerateBinaryCode(input, ast);
            }
            catch (CodeGenErrorsException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
