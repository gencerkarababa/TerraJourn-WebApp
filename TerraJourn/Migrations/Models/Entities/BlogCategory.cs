using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TerraJourn.Models.Entities
{
    public class BlogCategory
    {
        [Key]
        public int BlogCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string IconClass { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
