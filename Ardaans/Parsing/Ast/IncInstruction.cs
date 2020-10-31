using System;
using System.Collections.Generic;
using System.Text;

using Ardaans.Assembly;
using Ardaans.Tokens;

namespace Ardaans.Parsing.Ast
{
    public class IncInstruction : OneOperandNode
    {
        public IncInstruction(Input input, Token instructionToken, Token operand1)
            : base(input, instructionToken, operand1)
        {
        }

        public override bool GenerateCode(Action<OneOperandNode> logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();

            if (CodeGenerator.IsValidPattern(operand1Type, CodeGenerator.IncOpcodes))
            {
                byte opcode = CodeGenerator.IncOpcodes[operand1Type];

                code = CodeGenerator.Generate(opcode, operand1);
                return true;
            }
            else
            {
                logErrorFunc(this);
                return false;
            }
        }

        public override string ToString()
            => $"Node<Inc>{{ {this.operand1} }}";
    }
}
