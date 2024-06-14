﻿using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC016501Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC016501Highlighting.CheckId + ": " + SPC016501Highlighting.Message,
  "Required attributes ID, Name must be declared in FieldRef.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareRequiredAttributesInFieldRef : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "FieldRef")
            {
                result = !element.AttributeExists("ID") || !element.AttributeExists("Name");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC016501Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC016501Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldRef.SPC016501;
        public const string Message = "Declare required attributes in FieldRef";

        public SPC016501Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
