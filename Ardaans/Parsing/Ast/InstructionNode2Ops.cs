using System;

using Ardaans.Assembly;
using Ardaans.Tokens;

namespace Ardaans.Parsing.Ast
{
    public class InstructionNode2Ops : InstructionNode1Op
    {
        protected Token operand2;

        public InstructionNode2Ops(Input input, Token instructionToken, Token operand1, Token operand2)
            : base(input, instructionToken, operand1)
        {
            this.operand2 = operand2;
        }

        public override bool HasSamePattern(InstructionNode1Op other)
        {
            var other2Ops = other as InstructionNode2Ops;
            if (other2Ops == null)
                return false;

            Type op2Type = this.operand2.GetType();
            Type otherOp2Type = other2Ops.operand2.GetType();

            return base.HasSamePattern(other)
                && op2Type == otherOp2Type;
        }

        public override byte[] GenerateOperandOpcode()
        {
            return new byte[] { this.operand1.GenerateCode(), this.operand2.GenerateCode() };
        }
    }
}
