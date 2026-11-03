using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace TerraJourn.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gerekli.")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı 50 karakterden uzun olamaz.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email adresi gerekli.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi girin.")]
        public string Email { get; set; }

        [StringLength(255, MinimumLength = 6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}