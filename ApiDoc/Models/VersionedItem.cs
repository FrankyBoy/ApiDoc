using System;
using System.Collections.Generic;

namespace ApiDoc.Models
{
    public class VersionedItem
    {
        public int? Id { get; set; }
        public string Author { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeNote { get; set; }
        public long RevisionNumber { get; set; }
        public bool Deleted { get; set; }
        public IList<VersionedItem> Children { get; set; }
    }
}