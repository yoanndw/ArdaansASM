using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class JmpVal : InstructionGen
    {
        private NumericalValueToken val;

        public JmpVal(NumericalValueToken val)
        {
            this.val = val;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.val.GenerateCode();

            byte[] code = { 0x13, regCode };
            return code;
        }
    }
}
