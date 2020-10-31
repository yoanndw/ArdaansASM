using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ardaans.Parsing;
using Ardaans.Parsing.Ast;
using Ardaans.Tokens;

namespace Ardaans.Assembly
{
    using Ast = List<OneOperandNode>;
    using OneOperandInstructionOpcodesRef = Dictionary<Type, byte>;
    using TwoOperandsInstructionOpcodesRef = Dictionary<(Type, Type), byte>;

    public class CodeGenerator
    {
        #region Patterns
        public static Dictionary<string, List<string>> Patterns = new Dictionary<string, List<string>>()
        {
            // mov
            {
                "mov",
                new List<string>()
                {
                    "R\tV",
                    "R\t&V",
                    "&V\tR",
                    "R\t&R",
                    "&R\tR",
                    "R\tR"
                }
            },

            // add
            {
                "add",
                new List<string>()
                {
                    "R\tV",
                    "R\tR"
                }
            },

            // sub
            {
                "sub",
                new List<string>()
                {
                    "R\tV",
                    "R\tR"
                }
            },

            // mul
            {
                "mul",
                new List<string>()
                {
                    "R\tV",
                    "R\tR"
                }
            },

            // div
            {
                "div",
                new List<string>()
                {
                    "R\tV",
                    "R\tR"
                }
            },

            // cmp
            {
                "cmp",
                new List<string>()
                {
                    "R\tV",
                    "R\tR"
                }
            },

            // inc
            {
                "inc",
                new List<string>()
                {
                    "R"
                }
            },

            // dec
            {
                "dec",
                new List<string>()
                {
                    "R"
                }
            },

            // jmp
            {
                "jmp",
                new List<string>()
                {
                    "V"
                }
            },

            // jeq
            {
                "jeq",
                new List<string>()
                {
                    "V"
                }
            },

            // jne
            {
                "jne",
                new List<string>()
                {
                    "V"
                }
            },

            // jsm
            {
                "jsm",
                new List<string>()
                {
                    "V"
                }
            },

            // jns
            {
                "jns",
                new List<string>()
                {
                    "V"
                }
            }
        };
        #endregion

        #region Opcodes Definition
        // Mov
        public static TwoOperandsInstructionOpcodesRef MovOpcodes = new TwoOperandsInstructionOpcodesRef
        {
            // mov R V
            { (typeof(RegisterToken), typeof(NumericalValueToken)), 0x01 },

            // mov R A
            { (typeof(RegisterToken), typeof(AddressValueToken)), 0x02 },

            // mov A R
            { (typeof(AddressValueToken), typeof(RegisterToken)), 0x03 },

            // mov R &R
            { (typeof(RegisterToken), typeof(AddressRegisterToken)), 0x04 },

            // mov &R R
            { (typeof(AddressRegisterToken), typeof(RegisterToken)), 0x05 },

            // mov R R
            { (typeof(RegisterToken), typeof(RegisterToken)), 0x06 }
        };

        // Add
        public static TwoOperandsInstructionOpcodesRef AddOpcodes = new TwoOperandsInstructionOpcodesRef
        {
            // add R V
            { (typeof(RegisterToken), typeof(NumericalValueToken)), 0x07 },

            // add R R
            { (typeof(RegisterToken), typeof(RegisterToken)), 0x08 }
        };

        // Sub
        public static TwoOperandsInstructionOpcodesRef SubOpcodes = new TwoOperandsInstructionOpcodesRef
        {
            // sub R V
            { (typeof(RegisterToken), typeof(NumericalValueToken)), 0x09 },

            // sub R R
            { (typeof(RegisterToken), typeof(RegisterToken)), 0x0A }
        };

        // Mul
        public static TwoOperandsInstructionOpcodesRef MulOpcodes = new TwoOperandsInstructionOpcodesRef
        {
            // mul R V
            { (typeof(RegisterToken), typeof(NumericalValueToken)), 0x0B },

            // mul R R
            { (typeof(RegisterToken), typeof(RegisterToken)), 0x0C }
        };

        // Div
        public static TwoOperandsInstructionOpcodesRef DivOpcodes = new TwoOperandsInstructionOpcodesRef
        {
            // div R V
            { (typeof(RegisterToken), typeof(NumericalValueToken)), 0x0D },

            // div R R
            { (typeof(RegisterToken), typeof(RegisterToken)), 0x0E }
        };

        // Cmp
        public static TwoOperandsInstructionOpcodesRef CmpOpcodes = new TwoOperandsInstructionOpcodesRef
        {
            // cmp R V
            { (typeof(RegisterToken), typeof(NumericalValueToken)), 0x0F },

            // cmp R R
            { (typeof(RegisterToken), typeof(RegisterToken)), 0x10 }
        };

        // Inc
        public static OneOperandInstructionOpcodesRef IncOpcodes = new OneOperandInstructionOpcodesRef
        {
            // inc R
            { typeof(RegisterToken), 0x11 }
        };

        // Dec
        public static OneOperandInstructionOpcodesRef DecOpcodes = new OneOperandInstructionOpcodesRef
        {
            // dec R
            { typeof(RegisterToken), 0x12 }
        };

        // Jmp
        public static OneOperandInstructionOpcodesRef JmpOpcodes = new OneOperandInstructionOpcodesRef
        {
            // jmp V
            { typeof(NumericalValueToken), 0x13 }
        };

        // Jeq
        public static OneOperandInstructionOpcodesRef JeqOpcodes = new OneOperandInstructionOpcodesRef
        {
            // jeq V
            { typeof(NumericalValueToken), 0x14 }
        };

        // Jne
        public static OneOperandInstructionOpcodesRef JneOpcodes = new OneOperandInstructionOpcodesRef
        {
            // jne V
            { typeof(NumericalValueToken), 0x15 }
        };

        // Jsm
        public static OneOperandInstructionOpcodesRef JsmOpcodes = new OneOperandInstructionOpcodesRef
        {
            // jsm V
            { typeof(NumericalValueToken), 0x16 }
        };

        // Jns
        public static OneOperandInstructionOpcodesRef JnsOpcodes = new OneOperandInstructionOpcodesRef
        {
            // jns V
            { typeof(NumericalValueToken), 0x17 }
        };
        #endregion

        public static bool IsValidPattern(Type pattern, OneOperandInstructionOpcodesRef instructionRef)
        {
            return instructionRef.ContainsKey(pattern);
        }

        public static bool IsValidPattern((Type, Type) pattern, TwoOperandsInstructionOpcodesRef instructionRef)
        {
            return instructionRef.ContainsKey(pattern);
        }

        #region Instruction Code Generation
        public static byte[] Generate(byte instructionOpcode, Token operand)
        {
            return new byte[] { instructionOpcode, operand.GenerateCode() };
        }

        public static byte[] Generate(byte instructionOpcode, Token operand1, Token operand2)
        {
            return new byte[] { instructionOpcode, operand1.GenerateCode(), operand2.GenerateCode() };
        }
        #endregion

        // INSTANCE //
        private Input input;
        private Ast ast;

        private byte[] binaryCode;

        private int errorsCount;

        private CodeGenerator(Input input, Ast ast)
        {
            this.input = input;
            this.ast = ast;

            this.errorsCount = 0;
        }

        private void LogError(WrongPatternError err)
        {
            Console.WriteLine(err);
            this.errorsCount++;
        }

        public void LogError(OneOperandNode instruction)
        {
            this.LogError(new WrongPatternError(instruction));
        }

        public static byte[] GenerateBinaryCode(Input input, Ast ast)
        {
            var codeGenerator = new CodeGenerator(input, ast);
            codeGenerator.GenerateBinaryCode();

            return codeGenerator.binaryCode;
        }

        private void GenerateBinaryCode()
        {
            var lsCode = new List<byte[]>();
            foreach (OneOperandNode node in this.ast)
            {
                byte[] code;
                if (node.GenerateCode(this.LogError, out code))
                {
                    lsCode.Add(code);
                }
            }

            this.binaryCode = lsCode.SelectMany(code => code).ToArray(); // flattens

            if (this.errorsCount != 0)
            {
                Console.WriteLine(this.errorsCount + " errors found.");
                Console.WriteLine("---");

                throw new CodeGenErrorsException();
            }
        }
    }
}
