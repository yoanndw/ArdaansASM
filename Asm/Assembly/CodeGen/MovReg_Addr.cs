using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public class MovReg_Addr : InstructionGen
    {
        private RegisterToken reg;
        private AddressValueToken addr;

        public MovReg_Addr(RegisterToken reg, AddressValueToken addr)
        {
            this.reg = reg;
            this.addr = addr;
        }

        public override byte[] GenerateCode()
        {
            byte regCode = this.reg.GenerateCode();
            byte addrCode = this.addr.GenerateCode();

            byte[] code = { 0x02, regCode, addrCode };
            return code;
        }
    }
}
