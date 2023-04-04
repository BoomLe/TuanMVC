using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Blog.Models;
using App.Areas.Post.Models;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MVCTuan.Areas.Post.Controllers
{
   [Area("Post")]
    public class ViewPostControllers : Controller
    {
        private readonly ILogger<ViewPostControllers> _logger;
        private readonly MyDbContext _context;

        public ViewPostControllers(ILogger<ViewPostControllers> logger,MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("/post/{categoryslug?}")]
        public IActionResult Index(string categoryslug,[FromQuery(Name ="p")] int currentPage, int pagesize)
        {
            var categories = GetCategories();
            ViewBag.categories = categories;
            ViewBag.categoryslug = categoryslug;

            Category category = null;
            if(!string.IsNullOrEmpty(categoryslug))
            {
                category = _context.categoryModel.Where( p=> p.Slug == categoryslug)
                .Include(p => p.CategoryChildren)
                .FirstOrDefault();

                if(category == null)
                {
                    return NotFound("Không tìm thấy");
                }
            }

            var post = _context.Posts.Include(p => p.Author)
            .Include(p=> p.PostCategories)
            .ThenInclude(p => p.Category)
            .AsQueryable();

            post.OrderByDescending(p => p.DateUpdated);

            if(category != null)
            {
                var ids = new List<int>();
                category.ChildCategoryIDs(null, ids);
                ids.Add(category.Id);

                post = post.Where(p => p.PostCategories.Where(pc => ids.Contains(pc.CategoryID)).Any());
            }

              int totalPosts =  post.Count();
           if(pagesize <= 0) pagesize = 2;
           int countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

           if(currentPage > countPages) currentPage = countPages;
           if(currentPage < 1) currentPage = 1;
            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new{
                    p = pageNumber,
                    pagesize = pagesize
                })
            };

             var postsInPage = post.Skip((currentPage - 1) * pagesize)
            .Take(pagesize);
           


            ViewBag.PagingModel = pagingModel;
            ViewBag.totalPosts = totalPosts;

            ViewBag.category = category;
            return View(postsInPage.ToList());
        }

        [Route("/poat/{postslug}.html")]
        public IActionResult Detail(string postslug)
        {
            var categories =GetCategories();
            ViewBag.categories = categories;

            var post = _context.Posts.Where(p=> p.Slug == postslug)
            .Include(p=> p.Author)
            .Include(p => p.PostCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefault();

            if(post == null)
            {
                return NotFound("không tìm thấy bài");
            }
            Category category = post.PostCategories.FirstOrDefault()?.Category;
            ViewBag.category =category;

            var otherPost = _context.Posts.Where(p => p.PostCategories.Any(p => p.Category.Id == category.Id))
            .Where(p => p.PostId != post.PostId)
            .OrderByDescending(p => p.DateUpdated)
            .Take(2);

            ViewBag.otherPost = otherPost;
            return View(post);
        }

        private List<Category> GetCategories()
        {
            var categories = _context.categoryModel
            .Include(p => p.CategoryChildren)
            .AsEnumerable()
            .Where(p => p.ParentCategory == null)
            .ToList();
            return categories;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}