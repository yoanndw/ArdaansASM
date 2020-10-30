using System;
using System.IO;

using Asm.Assembly;

namespace Asm
{
    class Program
    {

        static void Main(string[] args)
        {
            //string program = @"inc #$5 mov #$0A a";
            try
            {
                byte[] code = Assembler.AssembleFile("C:/Users/yoann/Desktop/test.asm");

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
