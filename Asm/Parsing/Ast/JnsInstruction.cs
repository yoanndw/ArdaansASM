using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class JnsInstruction : OneOperandNode
    {
        public JnsInstruction(Token operand1)
            : base(operand1)
        {
        }

        public override byte[] GenerateCode()
        {
            Type operand1Type = this.operand1.GetType();

            byte opcode = CodeGenerator.JnsOpcodes[operand1Type];

            return CodeGenerator.GenerateForOneOperand(opcode, operand1);
        }

        public override string ToString()
            => $"Node<Jns>{{ {this.operand1} }}";
    }
}
