using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TerraJourn.Models.Entities
{
    public class AboutUs
    {
        [Key]
        public int AboutUsId { get; set; } 
        public string ImageUrl { get; set; } 
        public string Description { get; set; } 
    }
}