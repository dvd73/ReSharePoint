using System.Collections.Generic;
using JetBrains.Application.Settings;
using JetBrains.ReSharper.Daemon.Xml.Stages;
using JetBrains.ReSharper.Daemon.Xml.Stages.Analysis;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Util;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Pro.Tooltips.Common.XmlAnalysis
{
    [XmlAnalysisProvider]
    public class SPXmlSyntaxAnalysisProvider : IXmlAnalysisProvider
    {
        public bool OnlyPrimary => true;

        /// <summary>
        /// Check to see if it’s a file you’re interested in and add just the analyses you need
        /// </summary>
        /// <param name="file"></param>
        /// <param name="process"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IEnumerable<IXmlAnalysis> GetAnalyses(IXmlFile file, IDaemonProcess process, IContextBoundSettingsStore settings)
        {
            if (file is IDTDFile || file.GetSourceFile().HasExcluded(settings))
                return EmptyList<JetBrains.ReSharper.Daemon.Xml.Stages.XmlAnalysis>.InstanceList;

            return new JetBrains.ReSharper.Daemon.Xml.Stages.XmlAnalysis[]
            {
                new SPElementsFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new DisplayFeatureName(),
                    new DisplayListTypeName(),
                    new DisplayContentTypeName2(),
                    new DisplayWebTemplateConfigurationName()
                }),
                new SPListSchemaFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new DisplayContentTypeName() 
                }),
                new SPWebPartFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    
                }),
                new OnetFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    new DisplayFeatureName2(),
                    new DisplayListTypeName2() 
                }),
                new WebTempFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    
                }),
                new FldTypesFileTagProblemAnalysis(new ISPXmlTagProblemAnalyzer[]
                {
                    
                })
            };
        }
    }
}
