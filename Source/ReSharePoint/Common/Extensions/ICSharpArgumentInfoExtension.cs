using System.Collections.Generic;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;

namespace ReSharePoint.Common.Extensions
{
    public static class ICSharpArgumentInfoExtension
    {
        public static bool IsReferenceOfPropertyUsage(this ICSharpArgumentInfo argument, IClrTypeName typeName, IEnumerable<string> propertyNames)
        {
            bool result = false;
            
            if (argument is ExpressionArgumentInfo argumentInfo)
            {
                result = argumentInfo.Expression.IsResolvedAsPropertyUsage(typeName, propertyNames);
            }

            return result;
        }
    }
}
