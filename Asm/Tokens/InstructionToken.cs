using System;
using System.Collections.Generic;
using System.Text;

namespace Asm.Tokens
{
    public enum Instructions
    {
        Mov,
        Add, Sub, Mul, Div,
        Cmp,
        Inc, Dec,
        Jmp, Jeq, Jne, Jsm, Jns,
    }

    public class InstructionToken : Token
    {
        private static Dictionary<string, Instructions> keywords = new Dictionary<string, Instructions>
        {
            { "mov", Instructions.Mov },

            { "add", Instructions.Add },
            { "sub", Instructions.Sub },
            { "mul", Instructions.Mul },
            { "div", Instructions.Div },

            { "cmp", Instructions.Cmp },

            { "inc", Instructions.Inc },
            { "dec", Instructions.Dec },

            { "jmp", Instructions.Jmp },
            { "jeq", Instructions.Jeq },
            { "jne", Instructions.Jne },
            { "jsm", Instructions.Jsm },
            { "jns", Instructions.Jns }
        };

        public static bool IsKeywordValid(string keyword) => keywords.ContainsKey(keyword);

        public static Instructions GetTypeFromKeyword(string keyword) => keywords[keyword];

        public Instructions Instruction { get; private set; }

        public InstructionToken(int line, int pos, Instructions instruction)
            : base(line, pos)
        {
            this.Instruction = instruction;
        }

        // Never used for InstructionToken
        public override byte GenerateCode()
        {
            return 0x00;
        }

        public override string ToString() => $"Token<l.{this.Line} c.{this.Col}>[Instr: {this.Instruction}]";

        public override bool Equals(object obj)
        {
            var o = obj as InstructionToken;
            if (o == null)
                return false;

            return base.Equals(obj) && this.Instruction == o.Instruction;
        }
    }
}
