using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class JnsVal : InstructionGen
    {
        private NumericalValueToken val;

        public JnsVal(NumericalValueToken val)
        {
            this.val = val;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.val.GenerateCode();

            byte[] code = { 0x17, regCode };
            return code;
        }
    }
}
