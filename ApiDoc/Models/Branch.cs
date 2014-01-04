using System.Collections.Generic;
using ApiDoc.Utility;

namespace ApiDoc.Models
{
    public class Branch : Node
    {
        public IList<Node> Children { get; set; }

        public override string GetWikiUrlString()
        {
            return Name.ToWikiUrlString() + "/";
        }
    }
}