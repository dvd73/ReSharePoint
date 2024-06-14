using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Psi;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;

namespace ReSharePoint.Common.Extensions
{
    public static class IMethodExtension
    {
        /// <summary>
        /// Checks that method has same parameter list
        /// </summary>
        /// <param name="method">Method instance</param>
        /// <param name="parameters">Pass null if you don't want check parameters and not null else. Empty list means method has no parameters.</param>
        /// <returns></returns>
        public static bool HasSameParameters(this IMethod method, IEnumerable<ParameterCriteria> parameters)
        {
            bool result = true;

            if (parameters != null)
            {
                var parameterCriterias = parameters as ParameterCriteria[] ?? parameters.ToArray();
                if (method.Parameters.Count == parameterCriterias.Length && parameterCriterias.Length > 0)
                {
                    for (int i = 0; i < parameterCriterias.Length; i++)
                    {
                        var p = parameterCriterias[i];
                        var mp = method.Parameters[i];
                        result &= mp.Kind == p.Kind &&
                                  string.Equals(mp.Type.ToString(), p.ParameterType,
                                      StringComparison.OrdinalIgnoreCase);
                    }
                }
                //result =
                //    parameterCriterias.Any(
                //        p =>
                //            method.Parameters.All(
                //                mp =>
                //                    mp.Kind == p.Kind &&
                //                    string.Equals(mp.Type.ToString(), p.ParameterType,
                //                        StringComparison.OrdinalIgnoreCase)));
            }

            return result;
        }
    }
}
