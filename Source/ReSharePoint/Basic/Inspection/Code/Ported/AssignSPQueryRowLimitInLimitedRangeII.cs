using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050250IIHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050250Highlighting.CheckId + ": " + SPC050250IIHighlighting.Message,
  "Assign RowLimit for SPQuery within the limited range (default 1 to 2000).",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IMemberInitializer),
        HighlightingTypes = new[] {typeof (SPC050250IIHighlighting)})]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AssignSPQueryRowLimitInLimitedRangeII : SPElementProblemAnalyzer<IMemberInitializer>
    {
        protected override bool IsInvalid(IMemberInitializer element)
        {
            bool result = false;

            if (element.Reference.IsValid() && 
                element.Reference.GetName() == "RowLimit" && element.Expression?.ConstantValue.Value != null && element.Expression.ConstantValue.IsValid() && CheckSPQueryType(element.Reference.Resolve().Result.DeclaredElement))
            {
                int rowlimit = Convert.ToInt32(element.Expression.ConstantValue.Value.ToString());
                result = rowlimit < 1 || rowlimit > 2000;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IMemberInitializer element)
        {
            return new SPC050250IIHighlighting(element);
        }

        private bool CheckSPQueryType(IDeclaredElement declaredElement)
        {
            if (declaredElement == null) return false;

            if (declaredElement is IProperty property)
            {
                return property.GetContainingType().GetClrName().Equals(ClrTypeKeys.SPQuery);
            }
            else
            {
                return declaredElement.ToString().Contains("Property:" + ClrTypeKeys.SPQuery.FullName + ".RowLimit");
            }
        }
    }
    
    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050250IIHighlighting : SPCSharpErrorHighlighting<IMemberInitializer>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050250_2;
        public const string Message = "Assign RowLimit for SPQuery in limited range";

        public SPC050250IIHighlighting(IMemberInitializer element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
