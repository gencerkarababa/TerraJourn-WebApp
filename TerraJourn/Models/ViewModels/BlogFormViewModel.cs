using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TerraJourn.Models.Entities; 

namespace TerraJourn.Models.ViewModels
{
    public class BlogFormViewModel
    {
        public Blog Blog { get; set; }
        public List<AssignedCategoryData> Categories { get; set; }
    }
}