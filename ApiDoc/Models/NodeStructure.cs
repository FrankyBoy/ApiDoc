using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDoc.Models
{
    public class NodeStructure
    {
        public IList<Node> Nodes { get; set; }
        public string OriginalPath { get; set; }
        public bool AnyNodeDeleted { get { return Nodes.Any(x => x.Deleted); } }
        public bool AnyNodeRenamed {
            get {
                return !((OriginalPath == null && Nodes.Count == 1) ||
                    (OriginalPath != null && GetWikiPath().ToLower() == OriginalPath.ToLower()));
            }
        }

        public Node Last() { return Nodes.Last(); }

        public string GetWikiPath()
        {
            var output = new StringBuilder();
            foreach (var item in Nodes)
            {
                if (item.Id != 0)
                {
                    output.Append(item.GetWikiUrlString());
                }
            }

            return output.ToString();
        }
    }
}