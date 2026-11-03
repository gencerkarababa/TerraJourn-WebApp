using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TerraJourn.Models.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Text { get; set; } 
        public DateTime CreatedDate { get; set; }
        // --- Foreign Keys ---
        public int UserId { get; set; }
        public int BlogId { get; set; } 
        // --- Navigation Properties ---
        public virtual User User { get; set; } 
        public virtual Blog Blog { get; set; }
    }
}