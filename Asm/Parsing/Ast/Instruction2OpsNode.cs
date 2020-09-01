using Asm.Assembly.CodeGen;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public abstract class Instruction2OpsNode : InstructionNode
    {
        protected Token operand2;

        public Instruction2OpsNode(Token operand1, Token operand2)
            :base(operand1)
        {
            this.operand2 = operand2;
        }

        
    }
}
