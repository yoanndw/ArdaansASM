using System;
using System.Collections.Generic;

using Asm.Parsing;

namespace Asm.Assembly
{
    public class Assembler
    {
        private string program;

        private byte[] binaryCode;

        private Assembler(string program)
        {
            this.program = program;
        }

        public static byte[] Assemble(string program)
        {
            var asm = new Assembler(program);
            asm.Assemble();

            return asm.binaryCode;
        }

        private void Assemble()
        {
            var input = new Input(this.program);

            var tokens = Lexer.Tokenize(input);
            var ast = Parser.Parse(input, tokens);

            this.binaryCode = CodeGenerator.GenerateBinaryCode(ast);
        }
    }
}
