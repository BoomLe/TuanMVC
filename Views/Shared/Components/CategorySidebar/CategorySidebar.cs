using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Areas.Blog.Models;

namespace App.Component
{
        [ViewComponent]
        public class CategorySidebar : ViewComponent
        {
            public class CategorySidebarData {
                public List<Category> Categories {set;get;}
                public int level {set; get;}
                public string categoryslug {set; get;}
            }
            // public const string COMPONENTNAME = "CategorySidebar";
            
            // public CategorySidebar() {}
            public IViewComponentResult Invoke(CategorySidebarData data) {
                return View(data);
            }
        }
}