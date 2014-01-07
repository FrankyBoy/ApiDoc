using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiDoc.Utility;

namespace ApiDoc.Models
{
    public class Branch : Node
    {
        public IList<Node> Children { get; set; }


        [Required]
        public override string Name
        {
            get { return base.Name;  }
            set { base.Name = value; }
        }

        public override string GetWikiUrlString()
        {
            return Name.ToWikiUrlString() + "/";
        }
    }
}