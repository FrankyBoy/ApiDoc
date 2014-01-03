using System;
using System.ComponentModel.DataAnnotations;

namespace ApiDoc.Models
{
    public abstract class VersionedItem
    {
        // basic stuff
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        // structural info
        public int Id { get; set; }
        public int ParentId { get; set; }
        public bool Deleted { get; set; }

        // versioning stuff
        public string Author { get; set; }
        public DateTime ChangeDate { get; set; }
        [Required]
        public string ChangeNote { get; set; }
        public int RevisionNumber { get; set; }

        public abstract string GetWikiUrlString();
    }
}