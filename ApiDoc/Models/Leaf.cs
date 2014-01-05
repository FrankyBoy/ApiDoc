﻿using ApiDoc.Utility;

namespace ApiDoc.Models
{
    public class Leaf : Node
    {
        public string HttpVerb { get; set; }
        public bool RequiresAuthentication { get; set; }
        public bool RequiresAuthorization { get; set; }
        public string SampleRequest { get; set; }
        public string SampleResponse { get; set; }
        
        public override string GetWikiUrlString()
        {
            return string.Format("{0}_({1})", Name.ToWikiUrlString(), HttpVerb.ToUpper());
        }
    }
}