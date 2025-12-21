using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TerraJourn.Models.Entities
{
    public class PlaceCategory
    {
        [Key]
        public int PlaceCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } 

        [StringLength(500)]
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TouristAttraction> TouristAttractions { get; set; }
    }
}