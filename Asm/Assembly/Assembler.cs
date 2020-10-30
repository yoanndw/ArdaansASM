using System;
using System.Collections.Generic;

using Asm.Parsing;
using Asm.Parsing.Ast;
using Asm.Tokens;

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

            List<Token> tokens;
            try
            {
                tokens = Lexer.Tokenize(input);
            }
            catch (SyntaxErrorsException e)
            {
                Console.WriteLine(e.Message);
                throw new FailedAssemblingException(e);
            }

            List<OneOperandNode> ast;
            try
            {
                ast = Parser.Parse(input, tokens);
            }
            catch (ParseErrorsException e)
            {
                Console.WriteLine(e.Message);
                throw new FailedAssemblingException(e);
            }

            try
            {
                this.binaryCode = CodeGenerator.GenerateBinaryCode(input, ast);
            }
            catch(CodeGenErrorsException e)
            {
                Console.WriteLine(e.Message);
                throw new FailedAssemblingException(e);
            }
        }
    }
}
