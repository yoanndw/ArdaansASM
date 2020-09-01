using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class JeqVal : InstructionGen
    {
        private NumericalValueToken val;

        public JeqVal(NumericalValueToken val)
        {
            this.val = val;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.val.GenerateCode();

            byte[] code = { 0x14, regCode };
            return code;
        }
    }
}
