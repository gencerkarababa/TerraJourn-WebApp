using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TerraJourn.Models.Entities
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Başlık alanı boş bırakılamaz.")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "İçerik alanı boş bırakılamaz.")]
        public string Content { get; set; }

        [StringLength(500, ErrorMessage = "Resim URL'si en fazla 500 karakter olabilir.")]
        public string ImageUrl { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Blog yazarı belirtilmelidir.")]
        public int UserId { get; set; }

        // --- Navigation Properties ---
        public virtual User User { get; set; }
        public virtual ICollection<BlogCategory> BlogCategories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        // Constructor: CreatedDate otomatic.
        public Blog()
        {
            CreatedDate = DateTime.Now;
            BlogCategories = new HashSet<BlogCategory>();
            Comments = new HashSet<Comment>();
        }
    }
}
