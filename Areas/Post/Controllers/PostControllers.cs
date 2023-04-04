using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.Post.Models;

using Microsoft.AspNetCore.Authorization;
using App.Models;
using Microsoft.AspNetCore.Identity;
using App.Areas.Identity.Models;

namespace App.Areas.Post.Controllers
{
    [Area("Post")]
    [Route("admin/blog/post/[action]/{id?}")]
    // [Authorize(Roles = "Admin")]
    public class PostControllers : Controller
    {
        private readonly MyDbContext _context;
        private readonly UserManager<MyUserRole> _usermanger;

        public PostControllers(MyDbContext context,UserManager<MyUserRole> usermanger)
        {
            _context = context;
            _usermanger = usermanger;
        }

        // GET: PostControllers
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize)
        {
           var qr = _context.Posts.Include(p => p.Author).OrderByDescending(p => p.DateCreated);

           int totalPosts = await qr.CountAsync();
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
            ViewBag.PagingModel = pagingModel;
            ViewBag.totalPosts = totalPosts;
            ViewBag.pageIndex = (currentPage - 1)* pagesize;

            var postsInPage = await qr.Skip((currentPage - 1) * pagesize)
            .Take(pagesize)
            .Include(p => p.PostCategories)
            .ThenInclude(pc => pc.Category)
            .ToListAsync();

           return View(postsInPage);
        }

        // GET: PostControllers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // GET: PostControllers/Create
        public async Task <IActionResult> CreateAsync()
        {
           var categories = await _context.categoryModel.ToListAsync();

            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            return View();
        }

        // POST: PostControllers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published, CategoryIDs")] CreatedPostModel post)
        {
            var categories = await _context.categoryModel.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");


          
            if(await _context.Posts.AnyAsync(p => p.Slug == post.Slug))
            {
                ModelState.AddModelError("slug" , "nhập chuỗi url khác");
                return View(post);
            }
            if (ModelState.IsValid)
            {
                var user = await _usermanger.GetUserAsync(this.User);
                post.DateCreated = post.DateUpdated = DateTime.Now;
                // post.AuthorId = user.Id;
                _context.Add(post);

                if(post.CategoryIDs != null)
                {
                    foreach(var cateId in post.CategoryIDs)
                    {
                        _context.Add(new PostCategoryModel()
                        {
                            CategoryID = cateId,
                            Post = post
                        });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(post);
        }

        // GET: PostControllers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            // var postModel = await _context.Posts.FindAsync(id);
            var post = await _context.Posts.Include(p=> p.PostCategories).FirstOrDefaultAsync(p=> p.PostId ==id);
            if (post == null)
            {
                return NotFound();
            }

            var postEdit =  new CreatedPostModel()
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                Description = post.Description,
                Slug = post.Slug,
                Published = post.Published,
                CategoryIDs = post.PostCategories.Select(p => p.CategoryID).ToArray()
            };
          var categories = await _context.categoryModel.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");    
                    return View(postEdit);
        }

        // POST: PostControllers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Description,Slug,Content,Published, CategoryIDs")] CreatedPostModel postModel)
        {
            if (id != postModel.PostId)
            {
                return NotFound();
            }

          var categories = await _context.categoryModel.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");   


                 if(await _context.Posts.AnyAsync(p => p.Slug == postModel.Slug))
            {
                ModelState.AddModelError("slug" , "nhập chuỗi url khác");
                return View(postModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(postModel);
                    // await _context.SaveChangesAsync();

                    var postUpdate = await _context.Posts.Include(p=> p.PostCategories).FirstOrDefaultAsync(p=> p.PostId ==id);

                    if(postUpdate == null)
                    {
                        return NotFound();
                    }

                    postUpdate.Title = postModel.Title;
                    postUpdate.Description = postModel.Description;
                    postUpdate.Content = postModel.Content;
                    postUpdate.Published = postModel.Published;
                    postUpdate.Slug = postModel.Slug;
                    postUpdate.DateUpdated =DateTime.Now;

                    if(postModel.CategoryIDs == null) postModel.CategoryIDs = new int[]{};

                    var oldCateIds = postUpdate.PostCategories.Select( p=> p.CategoryID).ToArray();
                    var newCateIds = postModel.CategoryIDs;

                    var removeCAtePosts = from postCate in postUpdate.PostCategories
                    where(!newCateIds.Contains(postCate.CategoryID))
                    select postCate;

                    _context.PostCategories.RemoveRange(removeCAtePosts);

                    var addCateIds = from CateId in newCateIds
                    where !oldCateIds.Contains(CateId)
                    select CateId;

                    foreach (var cateId in addCateIds)
                    {
                        _context.PostCategories.Add(new PostCategoryModel()
                        {
                            PostID = id,
                            CategoryID =cateId
                        });
                        
                    }

                     _context.Update(postUpdate);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostModelExists(postModel.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", postModel.AuthorId);
            return View(postModel);
        }

        // GET: PostControllers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // POST: PostControllers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Lỗi không thể hoàn tất");
            }
            var postModel = await _context.Posts.FindAsync(id);
            if (postModel != null)
            {
                _context.Posts.Remove(postModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostModelExists(int id)
        {
          return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
