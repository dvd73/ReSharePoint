using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Util;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC040213Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC040213Highlighting.CheckId + ": " + SPC040213Highlighting.Message,
  "WebParts should be inherited from the ASP.NET Webpart.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IClassLikeDeclaration),
        HighlightingTypes = new[] {typeof (SPC040213Highlighting)})]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotInheritWebPartsFromSharePoint : SPElementProblemAnalyzer<IClassLikeDeclaration>
    {
        protected override bool IsInvalid(IClassLikeDeclaration element)
        {
            bool result = false;
            
            ITypeElement declaredClassTypeElement = element.DeclaredElement;

            if (declaredClassTypeElement is IClass)
            {
                var clrName = (declaredClassTypeElement as IClass).GetBaseClassType().GetClrName();
                if (clrName.Equals(ClrTypeKeys.SPWebPart))
                {
                    result = true;
                }
                else
                {
                    ITypeUsage parentClassDeclarationUsage = element.SuperTypeUsageNodes.FirstOrDefault();
                    if (parentClassDeclarationUsage != null)
                    {
                        IType parentClassType = CSharpTypeFactory.CreateType(parentClassDeclarationUsage);
                        IDeclaredElement parentClassDeclaredElement = null;
                        if (parentClassDeclarationUsage is IUserTypeUsage declaredTypeUsage2)
                        {
                            IReferenceName typeName = declaredTypeUsage2.ScalarTypeName;
                            if (typeName != null)
                                parentClassDeclaredElement = typeName.Reference.Resolve().DeclaredElement;
                        }
                        else
                            parentClassDeclaredElement = parentClassType.GetTypeElement();

                        if (parentClassDeclaredElement != null)
                        {
                            if (parentClassDeclaredElement is IClass parentClassDeclaredClass)
                            {
                                var parentClassDeclaredClrName = parentClassDeclaredClass.GetClrName();

                                if (!ClrTypeKeys.AllWebParts.Any(
                                    wp => parentClassDeclaredClrName.Equals(wp)))
                                {
                                    IEnumerable<IDeclaredType> parentTypes =
                                        parentClassDeclaredClass.GetAllSuperClasses();

                                    if (!ClrTypeKeys.AllWebParts.Any(
                                        wp => parentTypes.Any(parentType => parentType.GetClrName().Equals(wp))))
                                    {
                                        result =
                                            parentTypes.Any(
                                                parentType =>
                                                    parentType.GetClrName().Equals(ClrTypeKeys.SPWebPart)
                                            );
                                    }
                                }
                            }
                        }
                    }
                }

            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IClassLikeDeclaration element)
        {
            return new SPC040213Highlighting(element);
        }
        
    }
    
    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC040213Highlighting : SPCSharpErrorHighlighting<IClassLikeDeclaration>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC040213;
        public const string Message = "Do not inherit WebParts from Microsoft.SharePoint.WebPartPages.WebPart";

        public SPC040213Highlighting(IClassLikeDeclaration element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
