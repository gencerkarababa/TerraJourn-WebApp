using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TerraJourn.Models.Entities;

namespace TerraJourn.Models.ViewModels
{
    public class BlogDetailsViewModel
    {
        public Blog Blog { get; set; }
        public IEnumerable<BlogCategory> Categories { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}