using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using TerraJourn.Models.Entities;

namespace TerraJourn.Models.ViewModels
{
    public class BlogIndexViewModel
    {
        public IPagedList<Blog> PagedBlogs { get; set; }
        public IEnumerable<BlogCategory> Categories { get; set; }
    }
}