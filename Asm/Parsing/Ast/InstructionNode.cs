using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly.CodeGen;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public abstract class InstructionNode
    {
        protected Token operand1;

        public InstructionNode(Token operand1)
        {
            this.operand1 = operand1;
        }

        public abstract InstructionGen EvalOperandsType();

        public override abstract string ToString();
    }
}
