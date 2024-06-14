using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharePoint.Common.Extensions
{
    public static class IExpressionStatementExtension
    {
        public static bool IsOneOfTheTypes(this IExpressionStatement statement, IEnumerable<IClrTypeName> typeNames)
        {
            bool result = false;

            if (statement.Expression is IInvocationExpression expression)
            {
                var invokedExpression = expression.InvokedExpression;
                result |= invokedExpression.IsOneOfTypes(typeNames);
            }

            return result;
        }
        
        public static bool CheckExpression(this IExpressionStatement statement,
            Func<IExpressionStatement, bool> checkMethod,
            Func<IMethod, bool> checkNoBodyMethod, int deep)
        {
            bool result = false;

            if (deep < 0) return result;
            
            result |= checkMethod(statement);

            if (!result)
            {
                if (statement.Expression is IInvocationExpression expression)
                {
                    var invokedExpression =
                        expression.InvokedExpression;

                    IDeclaredElement referenceExpressionTarget = invokedExpression.ReferenceExpressionTarget();
                    if (referenceExpressionTarget is IMethod method)
                    {
                        var decls = method.GetDeclarations();
                        foreach (var methodDeclaration in decls)
                        {
                            var body = methodDeclaration.Children<IChameleonNode>().FirstOrDefault();
                            if (body != null)
                            {
                                foreach (IExpressionStatement child in body.Children<IExpressionStatement>())
                                {
                                    result |= CheckExpression(child, checkMethod, checkNoBodyMethod, deep - 1);

                                    if (result) break;
                                }
                            }

                            if (result) break;
                        }

                        if (!result && decls.Count == 0 && checkNoBodyMethod != null)
                        {
                            result |= checkNoBodyMethod(method);
                        }
                    }
                }
            }

            return result;
        }
    }
}
