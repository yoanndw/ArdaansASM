using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public abstract class OneOperandNode
    {
        public int Line { get; private set; }
        public int Col { get; private set; }

        protected Token operand1;

        public OneOperandNode(Token instructionToken, Token operand1)
        {
            this.Line = instructionToken.Line;
            this.Col = instructionToken.Col;

            this.operand1 = operand1;
        }

        public abstract bool GenerateCode(Action logErrorFunc, out byte[] code);

        public override abstract string ToString();
    }
}
