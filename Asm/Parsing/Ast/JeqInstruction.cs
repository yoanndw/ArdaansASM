﻿using System;
using System.Collections.Generic;
using System.Text;

using Asm.Assembly.CodeGen;
using Asm.Tokens;

namespace Asm.Parsing.Ast
{
    public class JeqInstruction : InstructionNode
    {
        public JeqInstruction(Token operand1)
            : base(operand1)
        {
        }

        public override InstructionGen EvalOperandsType()
        {
            dynamic newOperand1 = null;
            switch (this.operand1)
            {
                case NumericalValueToken o:
                    newOperand1 = o;
                    break;

                default:
                    //TODO: throw exception
                    break;
            }

            return InstructionGen.Jeq(newOperand1); // en fonction du type => différent type de mov
        }

        public override string ToString()
            => $"Node<Jeq>[Op1: {this.operand1}]";
    }
}
