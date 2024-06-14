using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharePoint.Common.Extensions
{
    public static class IReferenceExtension
    {
        public static bool IsResolvedAs(this IReference element, IEnumerable<string> fullReferenceNames)
        {
            bool result = false;

            if (element?.Resolve().DeclaredElement != null && element.Resolve().ResolveErrorType == ResolveErrorType.OK)
            {
                string method = element.Resolve().DeclaredElement.ToString();
                result = fullReferenceNames.Any(m => String.Equals(method, m, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        public static bool HasReferencesFromType(this IReference method,
            ITypeDeclaration classDeclaration)
        {
            return
                classDeclaration.ThisAndDescendants<IReferenceExpression>().ToEnumerable()
                    .Any(referenceExpression => referenceExpression.ContainsReference(method));
        }
    }
}
