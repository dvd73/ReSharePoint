using System;
using System.Text;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(MagicStringShouldNotBeUsedHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  MagicStringShouldNotBeUsedHighlighting.CheckId + ": Hardcoded magic string detected",
  "Do not use hardcoded public urls, pathes, emails and account names in code.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(ILiteralExpression), HighlightingTypes = new[] { typeof(MagicStringShouldNotBeUsedHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class MagicStringShouldNotBeUsed : SPElementProblemAnalyzer<ILiteralExpression>
    {
        [Flags]
        public enum ValidationResult : uint
        {
            Valid = 0,
            Url = 1,
            Path = 2,
            EMail = 4,
            AccountName = 8
        }

        private ValidationResult _validationResult = ValidationResult.Valid;

        protected override bool IsInvalid(ILiteralExpression element)
        {
            _validationResult = ValidationResult.Valid;

            IAttribute r = element.GetContainingNode<IAttribute>();

            if (r == null && element.ConstantValue.IsString())
            {
                string literal = element.ConstantValue.Value.ToString();
                switch (MagicStringsHelper.Match(literal))
                {
                    case "Uri":
                        _validationResult = ValidationResult.Url;
                        break;
                    case "Email":
                        _validationResult = ValidationResult.EMail;
                        break;
                    case "Path":
                        _validationResult = ValidationResult.Path;
                        break;
                    case "AccountName":
                        _validationResult = ValidationResult.AccountName;
                        break;
                    default:
                        break;
                }
            }

            return (uint)_validationResult > 0;
        }

        protected override IHighlighting GetElementHighlighting(ILiteralExpression element)
        {
            return new MagicStringShouldNotBeUsedHighlighting(element, _validationResult);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true)]
    public class MagicStringShouldNotBeUsedHighlighting : SPCSharpErrorHighlighting<ILiteralExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.MagicStringShouldNotBeUsed;
        
        public MagicStringShouldNotBeUsed.ValidationResult ValidationResult;

        private static string GetMessage(MagicStringShouldNotBeUsed.ValidationResult validationResult)
        {
            const string MESSAGE_TEMPLATE = "Hardcoded {0} is detected.";
            StringBuilder sb = new StringBuilder();

            foreach (var item in EnumUtil.GetValues<MagicStringShouldNotBeUsed.ValidationResult>())
            {
                if (item != MagicStringShouldNotBeUsed.ValidationResult.Valid &&
                    (validationResult & item) == item)
                {
                    sb.Append(String.Format(MESSAGE_TEMPLATE, item));
                }
            }

            return sb.ToString().Trim();
        }

        public MagicStringShouldNotBeUsedHighlighting(ILiteralExpression element, MagicStringShouldNotBeUsed.ValidationResult validationResult)
            : base(element, $"{CheckId}: {GetMessage(validationResult)}")
        {
            ValidationResult = validationResult;
        }
        
    }

}
