using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ApiDoc.Models
{
    public abstract class Node
    {
        public virtual string Name { get; set; }
        
        [UIHint("tinymce_full"), AllowHtml]
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

        public virtual string Title { get { return Name; } }

        public abstract string GetWikiUrlString();
    }
}