using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class JneVal : InstructionGen
    {
        private NumericalValueToken val;

        public JneVal(NumericalValueToken val)
        {
            this.val = val;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.val.GenerateCode();

            byte[] code = { 0x15, regCode };
            return code;
        }
    }
}
