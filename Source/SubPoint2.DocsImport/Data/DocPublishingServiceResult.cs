using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubPoint2.DocsImport.Data
{
    public class DocPublishingServiceResult
    {
        public DocPublishingServiceResult()
        {
            Pages = new List<WordpressPageDefinition>();
        }

        public List<WordpressPageDefinition> Pages { get; set; }

        public string PagesFolderPath { get; set; }
    }
}
