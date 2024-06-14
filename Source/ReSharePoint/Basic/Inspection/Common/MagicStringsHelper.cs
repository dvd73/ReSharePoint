using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReSharePoint.Basic.Inspection.Common
{
    public static class MagicStringsHelper
    { 
        private static readonly Dictionary<string, Regex> magicStringTypes= new Dictionary<string, Regex>
        {
            {"Uri", new Regex(@"((http|ftp|https):\/\/|www\.)[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?",RegexOptions.Compiled)},
            {"Email", new Regex(@"([a-z0-9_\.-]+)@([a-z0-9_\.-]+)\.([a-z\.]{2,6})",RegexOptions.Compiled)},
            {"Path", new Regex(@"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$",RegexOptions.Compiled)},
            {"AccountName", new Regex(@"^([a-z][a-z0-9.-]+)\\(?![\x20.]+$)([^\\/""[\]:|<>+=;,?*@]+)$",RegexOptions.Compiled)}
        };

        public static string Match(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return (from pair in magicStringTypes
                    where
                        pair.Value.IsMatch(value) &&
                        pair.Value.Matches(value).Cast<Match>().Any(x => !IsExcluded(x.Value))
                    select pair.Key).FirstOrDefault();
            }
            return null;
        }

        private static bool IsExcluded(string value)
        {
            //Exclude all schemas
            if (value.Trim().StartsWith("http://schemas.microsoft.com") ||
                value.Trim().StartsWith("http://www.w3.org"))
                return true;

            if (value.Trim().ToLower().Equals("sharepoint\\system"))
                return true;

            return false;
        }

        
    }
}
