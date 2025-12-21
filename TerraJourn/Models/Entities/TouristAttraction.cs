using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TerraJourn.Models.Entities
{
    public class TouristAttraction
    {
        [Key]
        public int TouristAttractionId { get; set; }
        [Required]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        // ---------- Navigation Properties ----------
        public virtual ICollection<PlaceCategory> PlaceCategories { get; set; }
    }
}