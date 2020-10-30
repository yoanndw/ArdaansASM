using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public abstract class OneOperandNode
    {
        protected Token operand1;

        public OneOperandNode(Token operand1)
        {
            this.operand1 = operand1;
        }

        public abstract bool GenerateCode(Action logErrorFunc, out byte[] code);

        public override abstract string ToString();
    }
}
