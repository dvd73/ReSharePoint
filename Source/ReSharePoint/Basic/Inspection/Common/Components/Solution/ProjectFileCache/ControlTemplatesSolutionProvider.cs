using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.Caches;
using JetBrains.Util;
using ReSharePoint.Common.Extensions;
using System.Text.RegularExpressions;
using JetBrains.Lifetimes;

namespace ReSharePoint.Basic.Inspection.Common.Components.Solution.ProjectFileCache
{
    [SolutionInstanceComponent]
    public class ControlTemplatesSolutionProvider : SPProjectFileItemCollectionDataProvider<ControlTemplateItem>
    {
        public ControlTemplatesSolutionProvider(Lifetime lifetime, ISolution solution, IProjectFileDataCache cache)
            : base(lifetime, solution, cache)
        {
        }

        public override ICollection<ControlTemplateItem> BuildData(VirtualFileSystemPath projectFileLocation, XmlDocument doc)
        {
            List<ControlTemplateItem> result = new List<ControlTemplateItem>();

            XDocument document = doc.ToXDocument();
            Regex regex = new Regex("^[ControlTemplates\\\\].+\\.ascx$");
            IEnumerable<XElement> controlItemNodes =
                document.Descendants()
                    .Where(p => p.Name.LocalName == "Content" && regex.IsMatch(p.Attribute("Include").Value));
            foreach (var node in controlItemNodes)
            {
                result.Add(new ControlTemplateItem()
                {
                    Path = Path.Combine(ProjectLocation, node.Attribute("Include").Value),
                    Include = node.Attribute("Include").Value
                });
            }
            
            return result;
        }
        public override int Version => 4;
    }

    [Serializable()]
    public class ControlTemplateItem : SPProjectFileCacheItem
    {
        public string Path { get; set; }
        public string Include { get; set; }
    }
}
