using System;
using System.Collections.Generic;
using System.Text;

namespace Asm
{
    enum OpcodeType
    {
        Invalid,

        // MOV
        Mov_Reg_Val,        // mov R V
        Mov_Reg_Addr,       // mov R [A]
        Mov_Addr_Reg,       // mov [A] R

        Mov_Reg1_Reg2Addr,  // mov R [R]
        Mov_Reg1Addr_Reg2,  // mov [R] R

        Mov_Reg1_Reg2,      // mov R R

        // ADD
        Add_RegVal,         // add R V
        Add_Reg1Reg2,       // add R R

        // SUB
        Sub_RegVal,         // sub R V
        Sub_Reg1Reg2,       // sub R R

        // MUL
        Mul_RegVal,         // mul R V
        Mul_Reg1Reg2,       // mul R R

        // DIV
        Div_RegVal,         // div R V
        Div_Reg1Reg2,       // div R R

        // CMP
        Cmp_RegVal,         // cmp R V
        Cmp_Reg1Reg2,       // cmp R R

        // JMP
        Jmp_Jmp,           // jmp A
        Jmp_Equals,        // je A
        Jmp_NotEquals,     // jne A
        Jmp_Smaller,       // js A
        Jmp_NotSmaller,    // jns A
    }

    class Opcode
    {
        private OpcodeType type;
        
        private int argsCount; // 1 or 2
        private byte arg1, arg2;

        public Opcode(OpcodeType type, byte arg1)
        {
            this.type = type;
            
            this.argsCount = 1;
            this.arg1 = arg1;
        }

        public Opcode(OpcodeType type, byte arg1, byte arg2)
        {
            this.type = type;

            this.argsCount = 2;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
    }
}
