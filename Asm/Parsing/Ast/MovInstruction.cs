using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly.CodeGen;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class MovInstruction : Instruction2OpsNode
    {
        public MovInstruction(Token operand1, Token operand2)
            : base(operand1, operand2)
        {
        }

        public override InstructionGen EvalOperandsType()
        {
            dynamic newOperand1 = null;
            switch (this.operand1)
            {
                case AddressRegisterToken o:
                    newOperand1 = o;
                    break;

                case RegisterToken o:
                    newOperand1 = o;
                    break;

                case AddressValueToken o:
                    newOperand1 = o;
                    break;

                default:
                    //TODO: throw exception
                    break;
            }

            // Future syntax
            /*newOperand1 = this.operand1 switch
            {
                AddressRegisterToken o => o,
                RegisterToken o => o,
                AddressValueToken => o
            };*/

            dynamic newOperand2 = null;
            switch (this.operand2)
            {
                case AddressRegisterToken o:
                    newOperand2 = o;
                    break;

                case RegisterToken o:
                    newOperand2 = o;
                    break;

                case AddressValueToken o:
                    newOperand2 = o;
                    break;

                case NumericalValueToken o:
                    newOperand2 = o;
                    break;

                default:
                    //TODO: throw exception
                    break;
            }

            return InstructionGen.Mov(newOperand1, newOperand2); // en fonction du type => différent type de mov
        }

        public override string ToString()
            => $"Node<Mov>[Op1: {this.operand1} Op2: {this.operand2}]";
    }
}
