using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class JsmVal : InstructionGen
    {
        private NumericalValueToken val;

        public JsmVal(NumericalValueToken val)
        {
            this.val = val;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.val.GenerateCode();

            byte[] code = { 0x16, regCode };
            return code;
        }
    }
}
