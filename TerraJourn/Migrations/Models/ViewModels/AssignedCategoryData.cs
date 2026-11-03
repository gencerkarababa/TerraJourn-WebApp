using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TerraJourn.Models.Entities;


namespace TerraJourn.Models.ViewModels
{
    public class AssignedCategoryData
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}