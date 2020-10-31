using System;
using System.Collections;

namespace Ardaans
{
    class VirtualMachine
    {
        enum Flags
        {
            Equals = 0,
            Smaller = 1,
        }

        private byte[] registers;

        private BitArray flags;

        private byte[] progMemory;
        private byte[] storageMemory;

        public byte[] StorageMemory
        {
            get => this.storageMemory;
        }

        private byte instructionPointer;

        public VirtualMachine()
        {
            this.registers = new byte[] { 0x00, 0x00, 0x00, 0x00 };

            this.flags = new BitArray(2);

            this.progMemory = new byte[256]; // 256
            this.storageMemory = new byte[256];

            this.instructionPointer = 0;
        }

        public VirtualMachine(byte[] program)
            : this()
        {
            this.LoadProgram(program);
        }

        public void LoadProgram(byte[] program)
        {
            for (int i = 0; i < this.progMemory.Length; i++)
            {
                // Reset old program
                if (i < program.Length)
                {
                    byte opcode = program[i];
                    this.progMemory[i] = opcode;
                }
                else
                {
                    this.progMemory[i] = 0x00;
                }
            }
        }

        public void Run()
        {
            while (!this.ReachedEnd())
            {
                byte opcode = this.Advance();

                try
                {
                    this.RunOpcode(opcode);
                }
                catch (OutOfProgramMemoryException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void PrintState()
        {
            Console.WriteLine("-----VM-----");
            Console.WriteLine("Registers");
            Console.WriteLine("A: {0:X2}", this.registers[0x00]);
            Console.WriteLine("B: {0:X2}", this.registers[0x01]);
            Console.WriteLine("C: {0:X2}", this.registers[0x02]);
            Console.WriteLine("D: {0:X2}", this.registers[0x03]);
            Console.WriteLine("\n--");
            Console.WriteLine("Flags");
            Console.WriteLine("Eq: {0}", this.GetFlag(Flags.Equals));
            Console.WriteLine("Sm: {0}", this.GetFlag(Flags.Smaller));
            Console.WriteLine("------------");
        }

        private byte GetRegisterValue(byte regCode)
        {
            return this.registers[regCode];
        }

        private bool GetFlag(Flags flag) => this.flags[(int)flag];

        private void SetFlag(Flags flag, bool value)
        {
            this.flags[(int)flag] = value;
        }

        private byte Advance()
        {
            byte opcode;
            try
            {
                opcode = this.progMemory[this.instructionPointer];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new OutOfProgramMemoryException(this.instructionPointer, e);
            }
            finally
            {
                this.instructionPointer++;
            }

            return opcode;
        }

        private byte Current
        {
            get => this.progMemory[this.instructionPointer];
        }

        private bool ReachedEnd() => this.instructionPointer >= this.progMemory.Length || this.Current == 0x00;

        private void RunOpcode(byte opcode)
        {
            switch (opcode)
            {
                // mov R V
                case 0x01:
                    {
                        byte reg = this.Advance();
                        byte val = this.Advance();

                        this.Opcode_Mov_Reg_Val(reg, val);
                        break;
                    }

                // mov R [A]
                case 0x02:
                    {
                        byte reg = this.Advance();
                        byte addr = this.Advance();

                        this.Opcode_Mov_Reg_Addr(reg, addr);
                        break;
                    }

                // mov [A] R
                case 0x03:
                    {
                        byte addr = this.Advance();
                        byte reg = this.Advance();

                        this.Opcode_Mov_Addr_Reg(addr, reg);
                        break;
                    }

                // mov R [R]
                case 0x04:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Mov_Reg1_Reg2Addr(reg1, reg2);
                        break;
                    }

                // mov [R] R
                case 0x05:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Mov_Reg1Addr_Reg2(reg1, reg2);
                        break;
                    }

                // mov R R
                case 0x06:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Mov_Reg1_Reg2(reg1, reg2);
                        break;
                    }

                // add R V
                case 0x07:
                    {
                        byte reg = this.Advance();
                        byte val = this.Advance();

                        this.Opcode_Add_RegVal(reg, val);
                        break;
                    }

                // add R R
                case 0x08:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Add_Reg1Reg2(reg1, reg2);
                        break;
                    }

                // sub R V
                case 0x09:
                    {
                        byte reg = this.Advance();
                        byte val = this.Advance();

                        this.Opcode_Sub_RegVal(reg, val);
                        break;
                    }

                // sub R R
                case 0x0A:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Sub_Reg1Reg2(reg1, reg2);
                        break;
                    }

                // mul R V
                case 0x0B:
                    {
                        byte reg = this.Advance();
                        byte val = this.Advance();

                        this.Opcode_Mul_RegVal(reg, val);
                        break;
                    }

                // mul R R
                case 0x0C:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Mul_Reg1Reg2(reg1, reg2);
                        break;
                    }

                // div R V
                case 0x0D:
                    {
                        byte reg = this.Advance();
                        byte val = this.Advance();

                        this.Opcode_Div_RegVal(reg, val);
                        break;
                    }

                // div R R
                case 0x0E:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Div_Reg1Reg2(reg1, reg2);
                        break;
                    }

                // cmp R V
                case 0x0F:
                    {
                        byte reg = this.Advance();
                        byte val = this.Advance();

                        this.Opcode_Cmp_RegVal(reg, val);
                        break;
                    }

                // cmp R R
                case 0x10:
                    {
                        byte reg1 = this.Advance();
                        byte reg2 = this.Advance();

                        this.Opcode_Cmp_Reg1Reg2(reg1, reg2);
                        break;
                    }

                // inc R
                case 0x11:
                    {
                        byte reg = this.Advance();

                        this.Opcode_IncReg(reg);
                        break;
                    }

                // dec R
                case 0x12:
                    {
                        byte reg = this.Advance();

                        this.Opcode_DecReg(reg);
                        break;
                    }

                // jmp A
                case 0x13:
                    {
                        byte addr = this.Advance();

                        this.Opcode_Jump(addr);
                        break;
                    }

                // jeq A
                case 0x14:
                    {
                        byte addr = this.Advance();

                        this.Opcode_JumpEquals(addr);
                        break;
                    }

                // jne A
                case 0x15:
                    {
                        byte addr = this.Advance();

                        this.Opcode_JumpNotEquals(addr);
                        break;
                    }

                // jsm A
                case 0x16:
                    {
                        byte addr = this.Advance();

                        this.Opcode_JumpSmaller(addr);
                        break;
                    }

                // jns A
                case 0x17:
                    {
                        byte addr = this.Advance();

                        this.Opcode_JumpNotSmaller(addr);
                        break;
                    }

                default:
                    break;
            }
        }

        #region Opcodes
        // mov R V
        private void Opcode_Mov_Reg_Val(byte reg, byte value)
        {
            this.registers[reg] = value;
        }

        // mov R [A]
        private void Opcode_Mov_Reg_Addr(byte reg, byte address)
        {
            byte value = this.storageMemory[address];
            this.Opcode_Mov_Reg_Val(reg, value);

        }

        // mov [A] R
        private void Opcode_Mov_Addr_Reg(byte address, byte reg)
        {
            this.storageMemory[address] = this.GetRegisterValue(reg);
        }

        // mov R [R]
        private void Opcode_Mov_Reg1_Reg2Addr(byte reg1, byte reg2)
        {
            this.Opcode_Mov_Reg_Addr(reg1, this.GetRegisterValue(reg2));
        }

        // mov [R] R
        private void Opcode_Mov_Reg1Addr_Reg2(byte reg1, byte reg2)
        {
            this.Opcode_Mov_Addr_Reg(this.GetRegisterValue(reg1), reg2);
        }

        // mov R R
        private void Opcode_Mov_Reg1_Reg2(byte reg1, byte reg2)
        {
            this.Opcode_Mov_Reg_Val(reg1, this.GetRegisterValue(reg2));
        }

        // add R V
        private void Opcode_Add_RegVal(byte reg, byte value)
        {
            this.registers[reg] += value;
        }

        // add R R
        private void Opcode_Add_Reg1Reg2(byte reg1, byte reg2)
        {
            this.Opcode_Add_RegVal(reg1, this.GetRegisterValue(reg2));
        }

        // sub R V
        private void Opcode_Sub_RegVal(byte reg, byte value)
        {
            this.registers[reg] -= value;
        }

        // sub R R
        private void Opcode_Sub_Reg1Reg2(byte reg1, byte reg2)
        {
            this.Opcode_Sub_RegVal(reg1, this.GetRegisterValue(reg2));
        }

        // mul R V
        private void Opcode_Mul_RegVal(byte reg, byte value)
        {
            this.registers[reg] *= value;
        }

        // mul R R
        private void Opcode_Mul_Reg1Reg2(byte reg1, byte reg2)
        {
            this.Opcode_Mul_RegVal(reg1, this.GetRegisterValue(reg2));
        }

        // div R V
        private void Opcode_Div_RegVal(byte reg, byte value)
        {
            this.registers[reg] /= value;
        }

        // div R R
        private void Opcode_Div_Reg1Reg2(byte reg1, byte reg2)
        {
            this.Opcode_Div_RegVal(reg1, this.GetRegisterValue(reg2));
        }

        // cmp R V
        private void Opcode_Cmp_RegVal(byte reg, byte value)
        {
            // Reset cmp result flags
            this.SetFlag(Flags.Equals, false);
            this.SetFlag(Flags.Smaller, false);

            byte regValue = this.GetRegisterValue(reg);
            if (regValue == value)
            {
                this.SetFlag(Flags.Equals, true);
            }
            else if (regValue < value)
            {
                this.SetFlag(Flags.Smaller, true);
            }
        }

        // cmp R R
        private void Opcode_Cmp_Reg1Reg2(byte reg1, byte reg2)
        {
            this.Opcode_Cmp_RegVal(reg1, this.GetRegisterValue(reg2));
        }

        // inc R
        private void Opcode_IncReg(byte reg)
        {
            this.registers[reg]++;
        }

        // dec R
        private void Opcode_DecReg(byte reg)
        {
            this.registers[reg]--;
        }

        // jmp A
        private void Opcode_Jump(byte address)
        {
            this.instructionPointer = address;
        }

        // jeq A
        private void Opcode_JumpEquals(byte address)
        {
            if (this.GetFlag(Flags.Equals))
                this.Opcode_Jump(address);
        }

        // jne A
        private void Opcode_JumpNotEquals(byte address)
        {
            if (!this.GetFlag(Flags.Equals))
                this.Opcode_Jump(address);
        }

        // jsm A
        private void Opcode_JumpSmaller(byte address)
        {
            if (this.GetFlag(Flags.Smaller))
                this.Opcode_Jump(address);
        }

        // jns A
        private void Opcode_JumpNotSmaller(byte address)
        {
            if (!this.GetFlag(Flags.Smaller))
                this.Opcode_Jump(address);
        }
        #endregion
    }
}
