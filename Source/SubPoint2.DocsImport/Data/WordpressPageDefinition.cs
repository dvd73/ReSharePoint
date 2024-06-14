using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubPoint2.DocsImport.Data
{
    public class WordpressPageDefinition
    {
        public WordpressPageDefinition()
        {
            Terms = new List<int>();
        }

        public string Title { get; set; }
        public string Name { get; set; }

        public string Content { get; set; }

        public string ParentPageId { get; set; }

        public bool IsTodoPage { get; set; }

        public List<int> Terms { get; set; }
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Name, ParentPageId, Title);
        }
    }
}
