using System.Linq;
using System.Text;

namespace Ardaans
{
    static class StringExtension
    {
        public static string EmphasizeChar(this string s, int charPos)
        {
            var sb = new StringBuilder(s + "\n");
            string spaces = string.Concat(Enumerable.Repeat(" ", charPos));

            sb.Append(spaces + "^");

            return sb.ToString();
        }
    }
}
