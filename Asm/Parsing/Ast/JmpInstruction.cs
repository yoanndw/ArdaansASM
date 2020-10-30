using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class JmpInstruction : OneOperandNode
    {
        public JmpInstruction(Input input, Token instructionToken, Token operand1)
            : base(input, instructionToken, operand1)
        {
        }

        public override bool GenerateCode(Action<OneOperandNode> logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();

            if (CodeGenerator.IsValidPattern(operand1Type, CodeGenerator.JmpOpcodes))
            {
                byte opcode = CodeGenerator.JmpOpcodes[operand1Type];

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
            => $"Node<Jmp>{{ {this.operand1} }}";
    }
}
