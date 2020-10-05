using System;
using System.Collections.Generic;

using Asm.Parsing;

namespace Asm.Assembly
{
    public class Assembler
    {
        /*private string program;

        private Lexer lexer;

        public Assembler(string program)
        {
            this.program = program;
            this.lexer = new Lexer(this.program);
        }

        public void LoadProgram(string program)
        {
            this.program = program;
            this.lexer = new Lexer(this.program);
        }

        public byte[] Assemble()
        {
            var tokens = this.lexer.Tokenize();

            var parser = new Parser(tokens);
            var ast = parser.GenerateAst();

            var codeGenerator = new CodeGenerator(ast);
            
            return codeGenerator.GenerateCode();
        }*/
    }
}
