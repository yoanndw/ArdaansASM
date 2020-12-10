using System;
using System.Collections.Generic;

using Ardaans.Parsing.Ast;
using Ardaans.Tokens;

namespace Ardaans.Parsing
{
    public class Parser
    {
        private Input rawInput;

        private List<Token> source;
        private int current;

        private List<InstructionNode1Op> nodes;

        private int errorsCount;

        private Parser(Input rawInput, List<Token> source)
        {
            this.rawInput = rawInput;
            this.source = source;
            this.current = 0;
            this.nodes = new List<InstructionNode1Op>();

            this.errorsCount = 0;
        }

        private void LogError(ParseError err)
        {
            this.errorsCount++;
            Console.WriteLine(err);
        }

        private void LogExpectedOneOperandError(Token token)
        {
            var err = new ExpectedOneOperandError(this.rawInput, token);
            this.LogError(err);
        }

        private void LogExpectedTwoOperandsError(Token token)
        {
            var err = new ExpectedTwoOperandsError(this.rawInput, token);
            this.LogError(err);
        }

        private Token Advance() => this.source[this.current++];

        private bool IsAtEnd() => this.current >= this.source.Count;

        public static List<InstructionNode1Op> Parse(Input input, List<Token> source)
        {
            var parser = new Parser(input, source);
            parser.GenerateAst();
            return parser.nodes;
        }

        private void GenerateAst()
        {
            while (!this.IsAtEnd())
            {
                InstructionNode1Op currentInstruction;
                if (this.GenerateNode(out currentInstruction))
                {
                    this.nodes.Add(currentInstruction);
                }
            }

            if (this.errorsCount != 0)
            {
                Console.WriteLine(this.errorsCount + " errors found.");
                Console.WriteLine("---");

                throw new ParseErrorsException();
            }
        }

        private bool GenerateNode(out InstructionNode1Op node)
        {
            node = null;

            InstructionToken instructionToken;
            if (this.ExpectInstructionToken(out instructionToken))
            {
                Instructions instruction = instructionToken.Instruction;
                switch (instruction)
                {
                    case Instructions.Mov:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                node = new MovInstruction(this.rawInput, instructionToken, operand1, operand2);
                            }
                            else
                            {
                                this.LogExpectedTwoOperandsError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Add:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                node = new AddInstruction(this.rawInput, instructionToken, operand1, operand2);
                            }
                            else
                            {
                                this.LogExpectedTwoOperandsError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Sub:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                node = new SubInstruction(this.rawInput, instructionToken, operand1, operand2);
                            }
                            else
                            {
                                this.LogExpectedTwoOperandsError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Mul:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                node = new MulInstruction(this.rawInput, instructionToken, operand1, operand2);
                            }
                            else
                            {
                                this.LogExpectedTwoOperandsError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Div:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                node = new DivInstruction(this.rawInput, instructionToken, operand1, operand2);
                            }
                            else
                            {
                                this.LogExpectedTwoOperandsError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Cmp:
                        {
                            Token operand1;
                            Token operand2;
                            if (this.ExpectTwoOperands(out operand1, out operand2))
                            {
                                node = new CmpInstruction(this.rawInput, instructionToken, operand1, operand2);
                            }
                            else
                            {
                                this.LogExpectedTwoOperandsError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Inc:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                node = new IncInstruction(this.rawInput, instructionToken, operand);
                            }
                            else
                            {
                                this.LogExpectedOneOperandError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Dec:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                node = new DecInstruction(this.rawInput, instructionToken, operand);
                            }
                            else
                            {
                                this.LogExpectedOneOperandError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Jmp:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                node = new JmpInstruction(this.rawInput, instructionToken, operand);
                            }
                            else
                            {
                                this.LogExpectedOneOperandError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Jeq:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                node = new JeqInstruction(this.rawInput, instructionToken, operand);
                            }
                            else
                            {
                                this.LogExpectedOneOperandError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Jne:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                node = new JneInstruction(this.rawInput, instructionToken, operand);
                            }
                            else
                            {
                                this.LogExpectedOneOperandError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Jsm:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                node = new JsmInstruction(this.rawInput, instructionToken, operand);
                            }
                            else
                            {
                                this.LogExpectedOneOperandError(instructionToken);
                                return false;
                            }
                        }
                        break;

                    case Instructions.Jns:
                        {
                            Token operand;
                            if (this.ExpectOneOperand(out operand))
                            {
                                node = new JnsInstruction(this.rawInput, instructionToken, operand);
                            }
                            else
                            {
                                this.LogExpectedOneOperandError(instructionToken);
                                return false;
                            }
                        }
                        break;
                }
            }

            return true;
        }

        public bool ExpectInstructionToken(out InstructionToken instructionToken)
        {
            instructionToken = null;
            if (this.IsAtEnd())
            {
                return false;
            }

            var token = this.Advance() as InstructionToken;
            if (token == null)
            {
                return false;
            }

            instructionToken = token;
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
    }
}
