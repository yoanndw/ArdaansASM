using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Asm.Parsing.Ast;
using Asm.Tokens;

namespace Asm.Parsing
{
    public class Parser
    {
        private List<Token> source;
        private int current;

        private List<OneOperandNode> nodes;

        private Token Current { get => this.source[this.current]; }

        private Parser(List<Token> source)
        {
            this.source = source;
            this.current = 0;
            this.nodes = new List<OneOperandNode>();
        }

        /*public List<InstructionNode> GenerateAst()
        {
            var ast = new List<InstructionNode>();

            while (!this.IsAtEnd())
            {
                var tok = this.Advance();
                ast.Add(this.GenerateNode(tok));
            }

            return ast;
        }*/

        private Token Advance() => this.source[this.current++];

        private bool IsAtEnd() => this.current >= this.source.Count;

        public static List<OneOperandNode> Parse(List<Token> source)
        {
            var parser = new Parser(source);
            parser.GenerateAst();
            return parser.nodes;
        }

        private void GenerateAst()
        {
            while (!this.IsAtEnd())
            {
                var currentInstruction = this.GenerateNode();
                if (currentInstruction != null)
                {
                    this.nodes.Add(currentInstruction);
                }
                else
                {
                    break;
                }
            }
        }

        private OneOperandNode GenerateNode()
        {
            Instructions instruction;
            if (this.ExpectInstruction(out instruction))
            {
                switch (instruction)
                {
                    case Instructions.Mov:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                return new MovInstruction(operand1, operand2);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Add:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                return new AddInstruction(operand1, operand2);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Sub:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                return new SubInstruction(operand1, operand2);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Mul:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                return new MulInstruction(operand1, operand2);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Div:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                return new DivInstruction(operand1, operand2);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Cmp:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                return new CmpInstruction(operand1, operand2);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Inc:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                return new IncInstruction(operand);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Dec:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                return new DecInstruction(operand);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Jmp:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                return new JmpInstruction(operand);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Jeq:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                return new JeqInstruction(operand);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Jne:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                return new JneInstruction(operand);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Jsm:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                return new JsmInstruction(operand);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    case Instructions.Jns:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                return new JnsInstruction(operand);
                            }
                            else
                            {
                                return null;
                            }
                        }

                    default:
                        return null;
                }
            }

            return null;
        }

        public bool ExpectInstruction(out Instructions instructionType)
        {
            instructionType = Instructions.Add;

            if (this.IsAtEnd())
            {
                return false;
            }

            var token = this.Advance() as InstructionToken;
            if (token == null)
            {
                return false;
            }

            instructionType = token.Instruction;
            return true;
        }

        public bool ExpectOneOperand(out Token operand)
        {
            operand = null;

            if (this.IsAtEnd())
            {
                return false;
            }

            var actualOperand = this.Advance();
            if (actualOperand is InstructionToken)
            {
                return false;
            }

            operand = actualOperand;
            return true;
        }

        public bool ExpectTwoOperands(out Token operand1, out Token operand2)
        {
            operand2 = null;
            return this.ExpectOneOperand(out operand1) && this.ExpectOneOperand(out operand2);
        }

        /*private InstructionNode GenerateNode(Token token)
        {
            var instructionToken = token as InstructionToken;
            if (instructionToken != null)
            {
                var instruction = instructionToken.Instruction;
                switch (instruction)
                {
                    case Instructions.Mov:
                        {
                            Token operand1 = this.Advance();
                            Token operand2 = this.Advance();

                            return new MovInstruction(operand1, operand2);
                        }

                    case Instructions.Add:
                        {
                            Token operand1 = this.Advance();
                            Token operand2 = this.Advance();

                            return new AddInstruction(operand1, operand2);
                        }

                    case Instructions.Sub:
                        {
                            Token operand1 = this.Advance();
                            Token operand2 = this.Advance();

                            return new SubInstruction(operand1, operand2);
                        }

                    case Instructions.Mul:
                        {
                            Token operand1 = this.Advance();
                            Token operand2 = this.Advance();

                            return new MulInstruction(operand1, operand2);
                        }

                    case Instructions.Div:
                        {
                            Token operand1 = this.Advance();
                            Token operand2 = this.Advance();

                            return new DivInstruction(operand1, operand2);
                        }

                    case Instructions.Cmp:
                        {
                            Token operand1 = this.Advance();
                            Token operand2 = this.Advance();

                            return new CmpInstruction(operand1, operand2);
                        }

                    case Instructions.Inc:
                        {
                            Token operand = this.Advance();

                            return new IncInstruction(operand);
                        }

                    case Instructions.Dec:
                        {
                            Token operand = this.Advance();

                            return new DecInstruction(operand);
                        }

                    case Instructions.Jmp:
                        {
                            Token operand = this.Advance();

                            return new JmpInstruction(operand);
                        }

                    case Instructions.Jeq:
                        {
                            Token operand = this.Advance();

                            return new JeqInstruction(operand);
                        }

                    case Instructions.Jne:
                        {
                            Token operand = this.Advance();

                            return new JneInstruction(operand);
                        }

                    case Instructions.Jsm:
                        {
                            Token operand = this.Advance();

                            return new JsmInstruction(operand);
                        }

                    case Instructions.Jns:
                        {
                            Token operand = this.Advance();

                            return new JnsInstruction(operand);
                        }

                    default:
                        break;
                }
            }
            else
            {
                //TODO: throw exception
            }

            return null;
        }*/


    }
}
