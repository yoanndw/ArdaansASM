﻿using System;
using System.Collections.Generic;
using System.Text;

using Ardaans.Assembly;
using Ardaans.Tokens;

namespace Ardaans.Parsing.Ast
{
    public class AddInstruction : TwoOperandsNode
    {
        public AddInstruction(Input input, Token instructionToken, Token operand1, Token operand2)
            : base(input, instructionToken, operand1, operand2)
        {
        }

        public override bool GenerateCode(Action<OneOperandNode> logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();
            Type operand2Type = this.operand2.GetType();

            if (CodeGenerator.IsValidPattern((operand1Type, operand2Type), CodeGenerator.AddOpcodes))
            {
                byte opcode = CodeGenerator.AddOpcodes[(operand1Type, operand2Type)];

                code = CodeGenerator.Generate(opcode, operand1, operand2);
                return true;
            }
            else
            {
                logErrorFunc(this);
                return false;
            }
        }

        public override string ToString()
            => $"Node<Add>{{ {this.operand1}, {this.operand2} }}";
    }
}
