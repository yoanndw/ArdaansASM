using System;
using System.Collections.Generic;
using System.Text;

using Ardaans.Assembly;
using Ardaans.Tokens;

namespace Ardaans.Parsing.Ast
{
    public class InstructionNode1Op
    {
        public int Line { get; private set; }
        public int Col { get; private set; }

        public string LineContent { get; private set; }

        public Instructions Instruction { get; private set; }

        protected Token operand1;

        public InstructionNode1Op(Input input, Token instructionToken, Token operand1)
        {
            this.Line = instructionToken.Line;
            this.Col = instructionToken.Col;

            if (input != null)
                this.LineContent = input.GetLine(this.Line);

            this.Instruction = (instructionToken as InstructionToken).Instruction;

            this.operand1 = operand1;
        }

        public virtual bool HasSamePattern(InstructionNode1Op other)
        {
            if (other == null)
                return false;

            Type op1Type = this.operand1.GetType();
            Type otherOp1Type = other.operand1.GetType();

            return this.Instruction == other.Instruction
                && op1Type == otherOp1Type;
        }

        public virtual byte[] GenerateOperandOpcode()
        {
            return new byte[] { this.operand1.GenerateCode() };
        }
    }
}
