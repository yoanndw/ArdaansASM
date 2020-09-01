using System;
using System.Collections.Generic;
using System.Text;

namespace Asm
{
    abstract class VMRuntimeException : Exception
    {
        public VMRuntimeException() : base() { }

        public VMRuntimeException(string message) : base(message) { }

        public VMRuntimeException(string message, Exception inner) : base(message, inner) { }
    }

    class OutOfProgramMemoryException : VMRuntimeException
    {
        private int instructionPointer;

        public OutOfProgramMemoryException(int instructionPointer, Exception inner)
            : base($"Instruction pointer reached last program byte ({instructionPointer} > 255). The program might be too long, so an instruction operand was not encoded.", inner)
        {
            this.instructionPointer = instructionPointer;
        }
    }
}
