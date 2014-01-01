using System;

namespace ApiDoc.Models
{
    public abstract class VersionedItem
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ChangeNote { get; set; }
        public long RevisionNumber { get; set; }
        public bool Deleted { get; set; }

        public abstract string GetWikiUrlString();
    }
}