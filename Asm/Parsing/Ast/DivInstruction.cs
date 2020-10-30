using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class DivInstruction : TwoOperandsNode
    {
        public DivInstruction(Input input, Token instructionToken, Token operand1, Token operand2)
            : base(input, instructionToken, operand1, operand2)
        {
        }

        public override bool GenerateCode(Action<OneOperandNode> logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();
            Type operand2Type = this.operand2.GetType();

            if (CodeGenerator.IsValidPattern((operand1Type, operand2Type), CodeGenerator.DivOpcodes))
            {
                byte opcode = CodeGenerator.DivOpcodes[(operand1Type, operand2Type)];

                code = CodeGenerator.Generate(opcode, operand1, operand2);
                return true;
            }
            else
            {
                logErrorFunc(this);
                return false;
            }
        }

        public override string ToString()
            => $"Node<Div>{{ {this.operand1}, {this.operand2} }}";
    }
}
