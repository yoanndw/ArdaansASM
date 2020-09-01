using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly.CodeGen;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class AddInstruction : Instruction2OpsNode
    {
        public AddInstruction(Token operand1, Token operand2)
            : base(operand1, operand2)
        {
        }

        public override InstructionGen EvalOperandsType()
        {
            dynamic newOperand1 = null;
            switch (this.operand1)
            {
                case RegisterToken o:
                    newOperand1 = o;
                    break;

                default:
                    //TODO: throw exception
                    break;
            }

            dynamic newOperand2 = null;
            switch (this.operand2)
            {
                case NumericalValueToken o:
                    newOperand2 = o;
                    break;

                case RegisterToken o:
                    newOperand2 = o;
                    break;

                default:
                    //TODO: throw exception
                    break;
            }

            return InstructionGen.Add(newOperand1, newOperand2); // en fonction du type => différent type de mov
        }

        public override string ToString()
            => $"Node<Add>[Op1: {this.operand1} Op2: {this.operand2}]";
    }
}
