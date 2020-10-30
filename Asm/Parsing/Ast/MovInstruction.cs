using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class MovInstruction : TwoOperandsNode
    {
        public MovInstruction(Token operand1, Token operand2)
            : base(operand1, operand2)
        {
        }

        public override bool GenerateCode(Action logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();
            Type operand2Type = this.operand2.GetType();

            if (CodeGenerator.IsValidPattern((operand1Type, operand2Type), CodeGenerator.MovOpcodes))
            {
                byte opcode = CodeGenerator.MovOpcodes[(operand1Type, operand2Type)];

                code = CodeGenerator.Generate(opcode, operand1, operand2);
                return true;
            }
            else
            {
                logErrorFunc();
                return false;
            }
        }

        public override string ToString()
            => $"Node<Mov>{{ {this.operand1}, {this.operand2} }}";
    }
}
