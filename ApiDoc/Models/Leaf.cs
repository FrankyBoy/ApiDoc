using ApiDoc.Utility;

namespace ApiDoc.Models
{
    public class Leaf : Node
    {
        public string HttpVerb { get; set; }
        public bool RequiresAuthentication { get; set; }
        public bool RequiresAuthorization { get; set; }
        public string SampleRequest { get; set; }
        public string SampleResponse { get; set; }

        public override string Name { get
            {
                return string.Format("{0} ({1})", base.Name, HttpVerb.ToUpper());
            }
        }

        public override string GetWikiUrlString()
        {
            return Name.ToWikiUrlString();
        }
    }
}