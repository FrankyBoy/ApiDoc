using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDoc.Models
{
    public class HttpVerbs
    {
        private readonly IDictionary<int, string> _dict;

        public HttpVerbs(IDictionary<int, string> dict)
        {
            _dict = dict;
        }
        
        public string NameForId(int id)
        {
            if (_dict.ContainsKey(id))
                return _dict[id];
            return null;
        }

        public int IdForName(string name)
        {
            var tmp = _dict
                .DefaultIfEmpty(new KeyValuePair<int, string>(-1, ""))
                .FirstOrDefault(x => x.Value.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            
            return tmp.Key;
        }

        public string this[int key]
        {
            get { return NameForId(key); }
        }
    }
}