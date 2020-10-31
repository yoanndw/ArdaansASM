using System;
using System.Collections.Generic;
using System.Text;

using Ardaans.Assembly;
using Ardaans.Tokens;

namespace Ardaans.Parsing.Ast
{
    public abstract class OneOperandNode
    {
        public int Line { get; private set; }
        public int Col { get; private set; }

        public string LineContent { get; private set; }

        public Instructions Instruction { get; private set; }

        protected Token operand1;

        public OneOperandNode(Input input, Token instructionToken, Token operand1)
        {
            this.Line = instructionToken.Line;
            this.Col = instructionToken.Col;

            this.LineContent = input.GetLine(this.Line);

            this.Instruction = (instructionToken as InstructionToken).Instruction;

            this.operand1 = operand1;
        }

        public abstract bool GenerateCode(Action<OneOperandNode> logErrorFunc, out byte[] code);

        public override abstract string ToString();
    }
}
