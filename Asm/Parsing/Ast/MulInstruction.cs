using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class MulInstruction : TwoOperandsNode
    {
        public MulInstruction(Token operand1, Token operand2)
            : base(operand1, operand2)
        {
        }

        public override byte[] GenerateCode()
        {
            Type operand1Type = this.operand1.GetType();
            Type operand2Type = this.operand2.GetType();

            byte opcode = CodeGenerator.MulOpcodes[(operand1Type, operand2Type)];

            return CodeGenerator.GenerateForTwoOperands(opcode, operand1, operand2);
        }

        public override string ToString()
            => $"Node<Mul>{{ {this.operand1}, {this.operand2} }}";
    }
}
