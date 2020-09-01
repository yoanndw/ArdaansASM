using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Asm.Assembly.CodeGen;
using Asm.Parsing;
using Asm.Parsing.Ast;

namespace Asm.Assembly
{
    public class CodeGenerator
    {
        private List<InstructionNode> ast;

        public CodeGenerator(List<InstructionNode> ast)
        {
            this.ast = ast;
        }

        public byte[] GenerateCode()
        {
            var generators = this.CreateCodeGenerators();
            List<byte[]> code = generators.ConvertAll(gen => gen.GenerateCode());

            // Flattened = in 1D array
            byte[] finalCode = code.SelectMany(b => b).ToArray();

            return finalCode;
        }

        private List<InstructionGen> CreateCodeGenerators()
        {
            List<InstructionGen> bytes = this.ast.ConvertAll(node => node.EvalOperandsType());

            return bytes;
        }
    }
}
