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
            string program = @"[$g";
            var lex = new Lexer(program);

            var tokens = lex.Tokenize();
            lex.PrintErrors();

            /*var asm = new Assembler(program);
            byte[] bin = asm.Assemble();

            var vm = new VirtualMachine();
            vm.LoadProgram(bin);

            vm.Run();
            vm.PrintState();*/
        }
    }
}
