using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.Blog.Models;
using App.Models;

namespace App.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("admin/blog/category/[action]/{id?}")]
    public class Blog : Controller
    {
        private readonly MyDbContext _context;

        public Blog(MyDbContext context)
        {
            _context = context;
        }

        private void CreatePaces(List<Category>soure, List<Category>des, int level)
        {
            var paces = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in soure)
            {
                // category.Title = paces + " " + category.Title;
                des.Add(new Category()
                {
                    Id = category.Id,
                    Title = paces + " " + category.Title
                });
                if(category.CategoryChildren?.Count > 0)
                {
                    CreatePaces(category.CategoryChildren.ToList(), des, level  +1);
                }
                
            }
        }

        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var qr = (from a in _context.categoryModel select a)
            .Include(p => p.ParentCategory)
            .Include(p => p.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(p => p.ParentCategory == null).ToList();

            return View(categories);


            // var myDbContext = _context.Category.Include(c => c.ParentCategory);
            // return View(await myDbContext.ToListAsync());
        }

        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Blog/Create
        public async Task<IActionResult> CreateAsync()
        {
            var qr1 = (from qr in _context.categoryModel select qr)
            .Include(p=> p.ParentCategory)
            .Include(p=> p.CategoryChildren);

            var categorys = (await qr1.ToListAsync()).Where(p => p.ParentCategory==null).ToList();
         categorys.Insert(0, new Category
         {
            Id = -1,
            Title = "Không chọn danh mục bài đăng"
         });
         var item = new List<Category>();
         CreatePaces(categorys, item,0);
         var selectlist = new SelectList(categorys,"Id", "Title");
            ViewData["ParentCategoryId"] = selectlist;
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Slug,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                if(category.ParentCategoryId == -1) category.ParentCategoryId= null;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            var qr1 = (from qr in _context.categoryModel select qr)
            .Include(p=> p.ParentCategory)
            .Include(p=> p.CategoryChildren);

           
        var categorys = (await qr1.ToListAsync()).Where(p => p.ParentCategory==null).ToList();
         
         categorys.Insert(0, new Category
         {
            Id = -1,
            Title = "Không chọn danh mục bài đăng"
         });

        var item = new List<Category>();
         CreatePaces(categorys, item,0);

         var selectlist = new SelectList(categorys,"Id", "Title");

            ViewData["ParentCategoryId"] = selectlist;
            return View(category);
        }

        // GET: Blog/Edit/5
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }


     
            var qr1 = (from qr in _context.categoryModel select qr)
            .Include(p=> p.ParentCategory)
            .Include(p=> p.CategoryChildren);

           
        var categorys = (await qr1.ToListAsync()).Where(p => p.ParentCategory==null).ToList();
         
         categorys.Insert(0, new Category
         {
            Id = -1,
            Title = "Không chọn danh mục bài đăng"
         });

        var item = new List<Category>();
         CreatePaces(categorys, item,0);

         var selectlist = new SelectList(categorys,"Id", "Title");

            ViewData["ParentCategoryId"] = selectlist;


            return View(category);
        }

        // POST: Blog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
        {
          
            if (id != category.Id)
            {
                return NotFound();
            }
            
            // bool CanUpdate = true;


            if(category.ParentCategoryId ==category.Id)
            {
                ModelState.AddModelError(string.Empty , "Phải chọn thư mục khác");
                // CanUpdate =false;
            }


            // if (CanUpdate && category.ParentCategoryId != null)
            // {
            //     var chilCates = (from qr in _context.categoryModel select qr)
            //     .Include(p => p.CategoryChildren)
            //     .ToList()
            //     .Where(p => p.ParentCategoryId ==category.Id);


            //     Func<List<Category>, bool > CheckCateId = null;

            //     CheckCateId = (CateIds) =>
            //     {
            //         foreach (var CateId in CateIds)
            //         {
            //             Console.WriteLine(CateId.Title);
            //             if(CateId.Id == category.ParentCategoryId)
            //             {
            //                 CanUpdate = false;
            //                 ModelState.AddModelError(string.Empty, "Buộc chọn thư mục khác");
                            
            //                 return true;
            //             }
            //             if(CateId.CategoryChildren != null)
                        
            //             return CheckCateId(CateId.CategoryChildren.ToList());   
                        
                        
            //         }
            //         return false;
            //     };
            //     CheckCateId(chilCates.ToList()); 
            // };
  

            if (ModelState.IsValid && category.ParentCategoryId != category.Id)
            {
                try
                {
                    if(category.ParentCategoryId == -1)
                     category.ParentCategoryId =null;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
  
            var qr1 = (from qr in _context.categoryModel select qr)
            .Include(p=> p.ParentCategory)
            .Include(p=> p.CategoryChildren);

           
        var categorys = (await qr1.ToListAsync()).Where(p => p.ParentCategory==null).ToList();
         
         categorys.Insert(0, new Category
         {
            Id = -1,
            Title = "Không chọn danh mục bài đăng"
         });

        var item = new List<Category>();
         CreatePaces(categorys, item,0);

         var selectlist = new SelectList(categorys,"Id", "Title");

            ViewData["ParentCategoryId"] = selectlist;            
            
            return View(category);
        }
        

        // GET: Blog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = (from qr in _context.categoryModel select qr)
            .Include( p => p.CategoryChildren)
            .FirstOrDefault( p => p.Id == id);
            
            if(category== null)
            {
                return NotFound();

            }
            foreach (var cCategoryChildren in category.CategoryChildren)
            {
                cCategoryChildren.ParentCategoryId = category.ParentCategoryId;
                
            }

         
            _context.Category.Remove(category);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Category?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
