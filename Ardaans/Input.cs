using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ardaans
{
    public class Input
    {
        private string source;
        private string[] lines;

        public Input(string source)
        {
            this.source = source.Replace("\r", "").ToLower();
            this.lines = this.source.Split("\n");
        }

        public int Length
        {
            get => this.source.Length;
        }

        public char GetChar(int index) => this.source[index];

        /// <summary>
        /// Returns a specified line between 1 and number of lines
        /// </summary>
        /// <param name="line">Number of the line</param>
        /// <returns>The specified line</returns>
        public string GetLine(int line) => this.lines[line - 1];

        /// <summary>
        /// Emphasize a specified character
        /// </summary>
        /// <param name="line">Emphasized line's number</param>
        /// <param name="col">Emphasized character's position</param>
        /// <returns>The line, and an emphase onto the specified character</returns>
        public string EmphasizeChar(int line, int col)
        {
            var sb = new StringBuilder(this.GetLine(line) + "\n");
            string spaces = string.Concat(Enumerable.Repeat(" ", col));

            sb.Append(spaces + "^");

            return sb.ToString();
        }
    }
}
