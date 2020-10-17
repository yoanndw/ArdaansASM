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
            string program = @"mov a #$5";
            var tokens = Lexer.Tokenize(program);

            var ast = Parser.Parse(tokens);
            byte[] bin = CodeGenerator.GenerateBinaryCode(ast);

            var vm = new VirtualMachine();
            vm.LoadProgram(bin);
            vm.Run();
            vm.PrintState();
        }
    }
}
