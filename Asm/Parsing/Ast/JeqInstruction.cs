using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class JeqInstruction : OneOperandNode
    {
        public JeqInstruction(Token operand1)
            : base(operand1)
        {
        }

        public override byte[] GenerateCode()
        {
            Type operand1Type = this.operand1.GetType();

            byte opcode = CodeGenerator.JeqOpcodes[operand1Type];

            return CodeGenerator.GenerateForOneOperand(opcode, operand1);
        }

        public override string ToString()
            => $"Node<Jeq>{{ {this.operand1} }}";
    }
}
