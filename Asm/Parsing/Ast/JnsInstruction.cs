using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class JnsInstruction : OneOperandNode
    {
        public JnsInstruction(Token instructionToken, Token operand1)
            : base(instructionToken, operand1)
        {
        }

        public override bool GenerateCode(Action logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();

            if (CodeGenerator.IsValidPattern(operand1Type, CodeGenerator.JnsOpcodes))
            {
                byte opcode = CodeGenerator.JnsOpcodes[operand1Type];

                code = CodeGenerator.Generate(opcode, operand1);
                return true;
            }
            else
            {
                logErrorFunc();
                return false;
            }
        }

        public override string ToString()
            => $"Node<Jns>{{ {this.operand1} }}";
    }
}
