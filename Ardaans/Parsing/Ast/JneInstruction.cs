﻿using System;
using System.Collections.Generic;
using System.Text;

using Ardaans.Assembly;
using Ardaans.Tokens;

namespace Ardaans.Parsing.Ast
{
    public class JneInstruction : OneOperandNode
    {
        public JneInstruction(Input input, Token instructionToken, Token operand1)
            : base(input, instructionToken, operand1)
        {
        }

        public override bool GenerateCode(Action<OneOperandNode> logErrorFunc, out byte[] code)
        {
            code = null;

            Type operand1Type = this.operand1.GetType();

            if (CodeGenerator.IsValidPattern(operand1Type, CodeGenerator.JneOpcodes))
            {
                byte opcode = CodeGenerator.JneOpcodes[operand1Type];

                code = CodeGenerator.Generate(opcode, operand1);
                return true;
            }
            else
            {
                logErrorFunc(this);
                return false;
            }
        }

        public override string ToString()
            => $"Node<Jne>{{ {this.operand1} }}";
    }
}