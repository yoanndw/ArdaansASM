using System;
using System.Collections.Generic;
using System.Text;

namespace Ardaans.Tokens
{
    public enum Registers : byte
    {
        RegA = 0x00,
        RegB = 0x01,
        RegC = 0x02,
        RegD = 0x03
    }

    public class RegisterToken : Token
    {
        private static Dictionary<string, Registers> keywords = new Dictionary<string, Registers>
        {
            { "a", Registers.RegA },
            { "b", Registers.RegB },
            { "c", Registers.RegC },
            { "d", Registers.RegD }
        };

        public static bool IsKeywordValid(string keyword) => keywords.ContainsKey(keyword);

        public static Registers GetTypeFromKeyword(string keyword) => keywords[keyword];

        public Registers Register { get; protected set; }

        public RegisterToken(int line, int pos, Registers register)
            : base(line, pos)
        {
            this.Register = register;
        }

        public override byte GenerateCode()
        {
            return (byte)this.Register;
        }

        public override string ToString() => $"Token<l.{this.Line} c.{this.Col}>[Reg: {this.Register}]";

        public override string DocRepresentation() => "R";

        public override bool Equals(object obj)
        {
            var o = obj as RegisterToken;
            if (o == null)
                return false;

            return base.Equals(obj) && this.Register == o.Register;
        }
    }
}
