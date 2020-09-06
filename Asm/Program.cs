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
            string program = @"&d";
            var lex = new Lexer(program);

            /*try
            {
                Console.WriteLine(lex.ExpectNumber());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }*/

            lex.Tokenize().ForEach(Console.WriteLine);

            //lex.PrintErrors();

            /*var asm = new Assembler(program);
            byte[] bin = asm.Assemble();

            var vm = new VirtualMachine();
            vm.LoadProgram(bin);

            vm.Run();
            vm.PrintState();*/
        }
    }
}
