using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDoc.Models
{
    public class HttpVerbs : Dictionary<int, string>
    {
        public HttpVerbs(IDictionary<int, string> dict) : base(dict){}
        
        public string NameForId(int id)
        {
            if (ContainsKey(id))
                return base[id];
            return null;
        }

        public int IdForName(string name)
        {
            var tmp = this
                .DefaultIfEmpty(new KeyValuePair<int, string>(-1, ""))
                .FirstOrDefault(x => x.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            
            return tmp.Key;
        }
    }
}