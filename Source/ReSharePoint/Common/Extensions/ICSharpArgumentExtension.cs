using System.Collections.Generic;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharePoint.Common.Extensions
{
    public static class ICSharpArgumentExtension
    {
        public static bool IsReferenceOfPropertyUsage(this ICSharpArgument argument, IClrTypeName typeName, IEnumerable<string> propertyNames)
        {
            bool result = false;

            if (argument.Value is IReferenceExpression expression)
            {
                result = expression.IsResolvedAsPropertyUsage(typeName, propertyNames);
            }

            return result;
        }
    }
}
