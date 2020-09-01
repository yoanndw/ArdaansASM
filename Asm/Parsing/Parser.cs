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

        private Token Current { get => this.source[this.current]; }

        public Parser(List<Token> source)
        {
            this.source = source;
        }

        public List<InstructionNode> GenerateAst()
        {
            var ast = new List<InstructionNode>();

            while (!this.ReachedEnd())
            {
                var tok = this.Advance();
                ast.Add(this.GenerateNode(tok));
            }

            return ast;
        }

        private Token Advance() => this.source[this.current++];

        private bool ReachedEnd() => this.current >= this.source.Count;

        private InstructionNode GenerateNode(Token token)
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
        }

        
    }
}
