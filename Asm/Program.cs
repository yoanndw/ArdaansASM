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
        static string ReadAsmSource(string path)
        {
            return File.ReadAllText(path);
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("[ERROR] Expected file name");
                return;
            }

            string path = args[0];
            if (!File.Exists(path))
            {
                Console.WriteLine("[ERROR] Couldn't find file '{0}'", path);
                return;
            }

            string program = ReadAsmSource(path);

            try
            {
                byte[] bin = Assembler.Assemble(program);

                var vm = new VirtualMachine(bin);
                vm.Run();
                vm.PrintState();
            }
            catch (SyntaxErrorsException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
