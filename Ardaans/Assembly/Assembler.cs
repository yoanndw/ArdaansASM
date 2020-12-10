using System;
using System.Collections.Generic;
using System.IO;

using Ardaans.Parsing;
using Ardaans.Parsing.Ast;
using Ardaans.Tokens;

namespace Ardaans.Assembly
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

        public static byte[] AssembleFile(string path)
        {
            string program = File.ReadAllText(path);
            
            return Assemble(program);
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

            List<InstructionNode1Op> ast;
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
                this.binaryCode = CodeGenerator.GenerateCode(input, ast);
            }
            catch(CodeGenErrorsException e)
            {
                Console.WriteLine(e.Message);
                throw new FailedAssemblingException(e);
            }
        }
    }
}
