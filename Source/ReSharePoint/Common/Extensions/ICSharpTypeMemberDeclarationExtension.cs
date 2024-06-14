using System;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;

namespace ReSharePoint.Common.Extensions
{
    public static class ICSharpTypeMemberDeclarationExtension
    {
        public static bool HasPropertySet(this ICSharpTypeMemberDeclaration method, IClrTypeName typeName,
            string propertyName, string qualifier)
        {
            bool result = false;

            foreach (IAssignmentExpression assignmentExpression in method.ThisAndDescendants<IAssignmentExpression>())
            {
                if (assignmentExpression.Dest is IReferenceExpression referenceExpression)
                {
                    bool propertyIsUsed = referenceExpression.IsResolvedAsPropertyUsage(typeName,
                        new[] {propertyName});
                    ICSharpExpression extensionQualifier = referenceExpression.GetExtensionQualifier();
                    bool qualifierIsUsed = extensionQualifier != null && String.Equals(extensionQualifier.GetText(), qualifier, StringComparison.OrdinalIgnoreCase);

                    result = qualifierIsUsed && propertyIsUsed;
                        
                    if (result)
                        break;
                }
            }

            return result;
        }

        public static bool HasVarAssigmentWithMethodUsage(this ICSharpTypeMemberDeclaration method, string variableName, IClrTypeName typeName, MethodCriteria[] methodCriterias)
        {
            bool result = false;

            if (!String.IsNullOrEmpty(variableName))
            {
                foreach (IAssignmentExpression assignmentExpression in
                    method.ThisAndDescendants<IAssignmentExpression>())
                {
                    if (assignmentExpression.Dest is IReferenceExpression expression &&
                        expression.NameIdentifier.Name == variableName)
                    {
                        result = assignmentExpression.Source != null &&
                                 assignmentExpression.Source.IsResolvedAsMethodCall(typeName,
                                     methodCriterias);

                        if (result) break;
                    }
                }
            }

            return result;
        }
    }
}
