using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TerraJourn.Models.Entities
{
    public class ContactInformation
    {
        [Key]
        public int ContactInfoId { get; set; } 
        public string Address { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public double Latitude { get; set; } 
        public double Longitude { get; set; } 
    }
}