using System;
using System.Text.RegularExpressions;

namespace ReSharePoint.Common.Extensions
{
    public static class StringExtension
    {
        #region validators

        public static bool FindJQueryVariableByIndexOf(this string line)
        {
            return line.IndexOf("$.", StringComparison.OrdinalIgnoreCase) >= 0 ||
                line.IndexOf("$(", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool FindJQueryDocumentReadyByIndexOf(this string line)
        {
            Regex r1 = new Regex(@"\([\s]*?document[\s]*?\)\.ready", RegexOptions.IgnoreCase);
            Regex r2 = new Regex(@"\w*uery\b[\s]*?\([\s]*?function[\s]*?\([\s]*?\)", RegexOptions.IgnoreCase);
            Regex r3 = new Regex(@"\$[\s]*?\([\s]*?function[\s]*?\([\s]*?\)", RegexOptions.IgnoreCase);

            return r1.IsMatch(line) || r2.IsMatch(line) || r3.IsMatch(line);
        }
        #endregion

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
