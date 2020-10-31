using System;
using System.IO;

using Ardaans.Assembly;

namespace Ardaans
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(@"Expected file name. Please run:
./ardaans file");

                return;
            }

            string filePath = args[0]; // C:/Users/yoann/Desktop/test.asm

            try
            {
                byte[] code = Assembler.AssembleFile(filePath);

                var vm = new VirtualMachine(code);
                vm.Run();
                vm.PrintState();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FailedAssemblingException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
