using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Caches;
using JetBrains.Util;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.Components.Solution.ProjectFileCache
{
    [SolutionInstanceComponent]
    public class SandboxedSolutionProvider : SPProjectFileItemDataProvider<SandboxedOptions>
    {
        public SandboxedSolutionProvider(Lifetime lifetime, ISolution solution, IProjectFileDataCache cache) : 
            base(lifetime, solution, cache)
        {
        }

        /// <summary>
        /// Return an object that represents your cached value(s).
        /// This could be a boolean, or a class. Don’t store anything in your class fields. ReSharper will cache this value.
        /// If the object instance is different to the previously cached instance, your OnDataChanged method is called.
        /// </summary>
        /// <param name="projectFileLocation"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public override SandboxedOptions BuildData(VirtualFileSystemPath projectFileLocation, XmlDocument doc)
        {
            bool isSandboxed = false;

            XDocument document = doc.ToXDocument();
            XElement sandboxedSolutionNode = document.Descendants().FirstOrDefault(p => p.Name.LocalName == "SandboxedSolution");

            if (sandboxedSolutionNode != null && !String.IsNullOrEmpty(sandboxedSolutionNode.Value))
                isSandboxed = sandboxedSolutionNode.Value.Trim().ToLower() == "true";

            return new SandboxedOptions() { IsSandboxed = isSandboxed }; 
        }
        

        public override int Version => 4;
    }

    [Serializable()]
    public class SandboxedOptions : SPProjectFileCacheItem
    {
        public bool IsSandboxed { get; set; }
    }

}
