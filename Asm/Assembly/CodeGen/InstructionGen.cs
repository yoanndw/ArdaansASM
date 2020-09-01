using System;
using System.Collections.Generic;
using System.Text;

using Asm.Tokens;

namespace Asm.Assembly.CodeGen
{
    public abstract class InstructionGen
    {
        // mov R V
        public static MovReg_Val Mov(RegisterToken reg, NumericalValueToken val) 
            => new MovReg_Val(reg, val);

        // mov R [A]
        public static MovReg_Addr Mov(RegisterToken reg, AddressValueToken addr)
            => new MovReg_Addr(reg, addr);

        // mov [A] R
        public static MovAddr_Reg Mov(AddressValueToken addr, RegisterToken reg)
            => new MovAddr_Reg(addr, reg);

        // mov R [R]
        public static MovReg_AddrReg Mov(RegisterToken reg, AddressRegisterToken addrReg)
            => new MovReg_AddrReg(reg, addrReg);

        // mov [R] R
        public static MovAddrReg_Reg Mov(AddressRegisterToken addrReg, RegisterToken reg)
            => new MovAddrReg_Reg(addrReg, reg);

        // mov R R
        public static MovReg_Reg Mov(RegisterToken reg1, RegisterToken reg2)
            => new MovReg_Reg(reg1, reg2);

        // add R V
        public static AddReg_Val Add(RegisterToken reg1, NumericalValueToken val)
            => new AddReg_Val(reg1, val);

        // add R R
        public static AddReg_Reg Add(RegisterToken reg1, RegisterToken reg2)
            => new AddReg_Reg(reg1, reg2);

        // sub R V
        public static SubReg_Val Sub(RegisterToken reg1, NumericalValueToken val)
            => new SubReg_Val(reg1, val);

        // sub R R
        public static SubReg_Reg Sub(RegisterToken reg1, RegisterToken reg2)
            => new SubReg_Reg(reg1, reg2);

        // mul R V
        public static MulReg_Val Mul(RegisterToken reg1, NumericalValueToken val)
            => new MulReg_Val(reg1, val);

        // mul R R
        public static MulReg_Reg Mul(RegisterToken reg1, RegisterToken reg2)
            => new MulReg_Reg(reg1, reg2);

        // div R V
        public static DivReg_Val Div(RegisterToken reg1, NumericalValueToken val)
            => new DivReg_Val(reg1, val);

        // div R R
        public static DivReg_Reg Div(RegisterToken reg1, RegisterToken reg2)
            => new DivReg_Reg(reg1, reg2);

        // cmp R V
        public static CmpReg_Val Cmp(RegisterToken reg1, NumericalValueToken val)
            => new CmpReg_Val(reg1, val);

        // cmp R R
        public static CmpReg_Reg Cmp(RegisterToken reg1, RegisterToken reg2)
            => new CmpReg_Reg(reg1, reg2);

        // inc R
        public static IncReg Inc(RegisterToken reg)
            => new IncReg(reg);

        // dec R
        public static DecReg Dec(RegisterToken reg)
            => new DecReg(reg);

        // jmp V
        public static JmpVal Jmp(NumericalValueToken val)
            => new JmpVal(val);

        // jeq V
        public static JeqVal Jeq(NumericalValueToken val)
            => new JeqVal(val);

        // jne V
        public static JneVal Jne(NumericalValueToken val)
            => new JneVal(val);

        // jsm V
        public static JsmVal Jsm(NumericalValueToken val)
            => new JsmVal(val);

        // jns V
        public static JnsVal Jns(NumericalValueToken val)
            => new JnsVal(val);

        public abstract byte[] GenerateCode();
    }
}
