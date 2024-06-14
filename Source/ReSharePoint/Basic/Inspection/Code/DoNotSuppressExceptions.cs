using System;
using System.Linq;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(DoNotSuppressExceptionsHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotSuppressExceptionsHighlighting.CheckId + ": " + DoNotSuppressExceptionsHighlighting.Message,
  "Rethrow exception in catch(Exception) block using throw or catch specific exception.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(ICatchClause), HighlightingTypes = new[] { typeof(DoNotSuppressExceptionsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotSuppressExceptions : SPElementProblemAnalyzer<ICatchClause>
    {
        protected  override bool IsInvalid(ICatchClause element)
        {
            return ((element is IGeneralCatchClause ||
                     element.ExceptionType.GetClrName().Equals(ClrTypeKeys.SystemException)) &&
                    !element.Body.Statements.OfType<IThrowStatement>().Any());
        }

        protected override IHighlighting GetElementHighlighting(ICatchClause element)
        {
            return new DoNotSuppressExceptionsHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotSuppressExceptionsHighlighting : SPCSharpErrorHighlighting<ICatchClause>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotSuppressExceptions;
        public const string Message = "Do not suppress general exception";

        public DoNotSuppressExceptionsHighlighting(ICatchClause element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
