using System;
using System.Collections.Generic;
using System.Linq;
using ReSharePoint.Common.Extensions;
using JetBrains.Application.Settings;
using JetBrains.Metadata.Reader.API;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Options;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(ULSLoggingInCatchBlockHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  ULSLoggingInCatchBlockHighlighting.CheckId + ": " + ULSLoggingInCatchBlockHighlighting.Message,
  "Catch block should include ULS logging output or re-throw.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(ICatchClause), HighlightingTypes = new[] { typeof(ULSLoggingInCatchBlockHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class ULSLoggingInCatchBlock : SPElementProblemAnalyzer<ICatchClause>
    {
        // it could be IGeneralCatchClause or ISpecificCatchClause(with exception type)
        protected override bool IsInvalid(ICatchClause element)
        {
            bool result = false;
            IPsiSourceFile sourceFile = element.GetSourceFile();
            var services = sourceFile.GetSolution().GetPsiServices();
            var solutionLoggers = services.Symbols.GetSymbolScope(LibrarySymbolScope.FULL, true).GetPossibleInheritors("SPDiagnosticsServiceBase").Select(logger => logger.GetClrName());

            foreach (ICSharpStatement statement in element.Body.Statements)
            {
                if (statement is IThrowStatement)
                {
                    result = true;
                    break;
                }
                else if (statement is IExpressionStatement statement1)
                {
                    result = statement1.CheckExpression(
                        expressionStatement => expressionStatement.IsOneOfTheTypes(solutionLoggers
                            .Union(new[] {ClrTypeKeys.SPDiagnosticsServiceBase})) || IsIgnoredCall(expressionStatement),
                        method => IsLoggerMethod(method, solutionLoggers, services), 20);
                    
                    if (result) break;
                }
            }

            return !result;
        }

        protected override IHighlighting GetElementHighlighting(ICatchClause element)
        {
            return new ULSLoggingInCatchBlockHighlighting(element);
        }

        private bool IsIgnoredCall(IExpressionStatement statement)
        {
            bool result = false;

            if (Settings != null && statement.Expression is IInvocationExpression expression)
            {
                var invokedExpression = expression.InvokedExpression;
                IDeclaredElement referenceExpressionTarget = invokedExpression.ReferenceExpressionTarget();
                if (referenceExpressionTarget is IMethod method)
                {
                    ITypeElement containingType = method.GetContainingType();
                    if (containingType != null)
                    {
                        var fullName = containingType.GetClrName().FullName;
                        result = Settings.EnumIndexedValues(ReSharePointSettingsAccessor.LoggersMasks)
                            .Aggregate(result,
                                (current, loggerMask) =>
                                    current |
                                    fullName.Contains(loggerMask, StringComparison.InvariantCultureIgnoreCase));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Является ли метод вызовом внешнего логгера (из dll)
        /// </summary>
        /// <param name="method"></param>
        /// <param name="solutionLoggers"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        private bool IsLoggerMethod(IMethod method, IEnumerable<IClrTypeName> solutionLoggers, IPsiServices services)
        {
            bool result = false;
            ITypeElement containingType = method.GetContainingType();
            if (containingType != null)
            {
                var fullName = containingType.GetClrName().FullName;
                result |= solutionLoggers.Any(
                    solutionLogger =>
                        !solutionLogger.FullName.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) &&
                        solutionLogger.FullName == fullName);
            }

            return result;
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class ULSLoggingInCatchBlockHighlighting : SPCSharpErrorHighlighting<ICatchClause>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.ULSLoggingInCatchBlock;
        public const string Message = "Not logged exception";

        public ULSLoggingInCatchBlockHighlighting(ICatchClause element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
