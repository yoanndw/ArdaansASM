using Ardaans.Assembly;
using Ardaans.Tokens;

namespace Ardaans.Parsing.Ast
{
    public abstract class TwoOperandsNode : OneOperandNode
    {
        protected Token operand2;

        public TwoOperandsNode(Input input, Token instructionToken, Token operand1, Token operand2)
            :base(input, instructionToken, operand1)
        {
            this.operand2 = operand2;
        }
    }
}
