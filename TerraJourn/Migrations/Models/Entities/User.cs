using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TerraJourn.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı 50 karakterden uzun olamaz.")]
        [Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email adresi gerekli.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi girin.")]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public string Role { get; set; }

        // --- Navigation Properties ---
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}