using System.Collections.Generic;
using ApiDoc.Utility;

namespace ApiDoc.Models
{
    public class Node : VersionedItem
    {
        public string Description { get; set; }
        public IList<VersionedItem> Children { get; set; }

        public override string GetWikiUrlString()
        {
            return Name.ToWikiUrlString() + "/";
        }
    }
}