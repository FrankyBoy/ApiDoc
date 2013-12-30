using System;

namespace ApiDoc.Models
{
    public class VersionedItem
    {
        public string Author { get; set; }
        public DateTime ChangeDate { get; set; }
        public int RevisionNumber { get; set; }
        public bool Deleted { get; set; }
    }
}