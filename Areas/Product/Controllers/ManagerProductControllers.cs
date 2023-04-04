using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using App.Areas.Identity.Models;
using App.Models;
using App.Areas.Product.Models;
using App.Areas.Post.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("admin/product-manager/[action]/{id?}")]
    // [Authorize(Roles = "Admin")]
    public class ManagerProductControllers : Controller
    {
        private readonly MyDbContext _context;
        private readonly UserManager<MyUserRole> _usermanger;

        private readonly IWebHostEnvironment _env;

        

       
        public ManagerProductControllers(MyDbContext context,UserManager<MyUserRole> usermanger,IWebHostEnvironment env)
        {
            _context = context;
            _usermanger = usermanger;
            _env = env;
        }

        // GET: PostControllers
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize)
        {
           var qr = _context.ProductsModels.Include(p => p.Author).OrderByDescending(p => p.DateCreated);

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
            .Include(p => p.ProductInCategories)
            .ThenInclude(pc => pc.Category)
            .ToListAsync();

           return View(postsInPage);
        }

        // GET: PostControllers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductsModels == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductsModels
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // GET: PostControllers/Create
        public async Task <IActionResult> CreateAsync()
        {
           var categories = await _context.CategoriesProduct.ToListAsync();

            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            return View();
        }

        // POST: PostControllers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published, CategoryIDs, Price")] CreatedProductModel Product)
        {
            var categories = await _context.CategoriesProduct.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");


          
            if(await _context.ProductsModels.AnyAsync(p => p.Slug == Product.Slug))
            {
                ModelState.AddModelError("slug" , "nhập chuỗi url khác");
                return View(Product);
            }
            if (ModelState.IsValid)
            {
                var user = await _usermanger.GetUserAsync(this.User);
                Product.DateCreated = Product.DateUpdated = DateTime.Now;
                // post.AuthorId = user.Id;
                _context.Add(Product);

                if(Product.CategoryIDs != null)
                {
                    foreach(var cateId in Product.CategoryIDs)
                    {
                        _context.Add(new ProductInCategory()
                        {
                            CategoryID = cateId,
                            Products = Product
                        });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
            return View(Product);
        }

        // GET: PostControllers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductsModels == null)
            {
                return NotFound();
            }

            // var postModel = await _context.ProductsModels.FindAsync(id);
            var products = await _context.ProductsModels.Include(p=> p.ProductInCategories).FirstOrDefaultAsync(p=> p.ProductId ==id);
            if (products == null)
            {
                return NotFound();
            }

            var productEdit =  new CreatedProductModel()
            {
                ProductId = products.ProductId,
                Title = products.Title,
                Content = products.Content,
                Description = products.Description,
                Slug = products.Slug,
                Published = products.Published,
                CategoryIDs = products.ProductInCategories.Select(p => p.CategoryID).ToArray(),
                Price = products.Price
            };
          var categories = await _context.CategoriesProduct.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");    
                    return View(productEdit);
        }

        // POST: PostControllers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Title,Description,Slug,Content,Published, CategoryIDs, Price")] CreatedProductModel productModel)
        {
            if (id != productModel.ProductId)
            {
                return NotFound();
            }

          var categories = await _context.CategoriesProduct.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");   


        if(await _context.ProductsModels.AnyAsync(p => p.Slug == productModel.Slug))
            {
                ModelState.AddModelError("slug" , "nhập chuỗi url khác");
                return View(productModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // _context.Update(postModel);
                    // await _context.SaveChangesAsync();

                    var productUpdate = await _context.ProductsModels.Include(p=> p.ProductInCategories).FirstOrDefaultAsync(p=> p.ProductId ==id);

                    if(productUpdate == null)
                    {
                        return NotFound();
                    }

                    productUpdate.Title = productModel.Title;
                    productUpdate.Description = productModel.Description;
                    productUpdate.Content = productModel.Content;
                    productUpdate.Published = productModel.Published;
                    productUpdate.Slug = productModel.Slug;
                    productUpdate.DateUpdated =DateTime.Now;
                    productUpdate.Price = productModel.Price;

                    if(productModel.CategoryIDs == null) productModel.CategoryIDs = new int[]{};

                    var oldCateIds = productUpdate.ProductInCategories.Select( p=> p.CategoryID).ToArray();
                    var newCateIds = productModel.CategoryIDs;

                    var removeCateProduct = from postCate in productUpdate.ProductInCategories
                    where(!newCateIds.Contains(postCate.CategoryID))
                    select postCate;

                    _context.ProductsInCategory.RemoveRange(removeCateProduct);

                    var addCateIds = from CateId in newCateIds
                    where !oldCateIds.Contains(CateId)
                    select CateId;

                    foreach (var cateId in addCateIds)
                    {
                        _context.ProductsInCategory.Add(new ProductInCategory()
                        {
                            ProductID = id,
                            CategoryID =cateId
                        });
                        
                    }

                     _context.Update(productUpdate);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostModelExists(productModel.ProductId))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", productModel.AuthorId);
            return View(productModel);
        }

        // GET: PostControllers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductsModels == null)
            {
                return NotFound();
            }

            var postModel = await _context.ProductsModels
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductId == id);
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
            if (_context.ProductsModels == null)
            {
                return Problem("Lỗi không thể hoàn tất");
            }
            var postModel = await _context.ProductsModels.FindAsync(id);
            if (postModel != null)
            {
                _context.ProductsModels.Remove(postModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostModelExists(int id)
        {
          return (_context.ProductsModels?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        public class UploadOneFile
        {
            // [FileExtensions(Extensions="png,jpg,jpeg,gif")]
            [DataType(DataType.Upload)]
            [Required(ErrorMessage ="Vui lòng chọn file")]
            [DisplayName("Chọn FileUpload")]
            public IFormFile? UploadFile{set;get;}
        }

        [HttpGet]
        public IActionResult UploadPhoto(int id)
        {
            var products = _context.ProductsModels.Where(p=> p.ProductId == id)
            .Include(p => p.Photos)
            .FirstOrDefault();

            if(products == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            ViewData["product"] = products;
            return View(new UploadOneFile());
        }

        [HttpPost, ActionName("UploadPhoto")]
        public async Task<IActionResult> UploadPhotoAsync(int id,[Bind("UploadFile")] UploadOneFile file)
        {
            var products = _context.ProductsModels.Where(p=> p.ProductId == id)
            .Include(p => p.Photos)
            .FirstOrDefault();

            if(products == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            ViewData["product"] = products;

            if(file != null)
            {
                var files = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                + Path.GetExtension(file.UploadFile.FileName);

              

                // var filePath = Path.Combine(_env.WebRootPath, "UploadImage/Products", files);
                var filePath = Path.Combine("UploadImage/Products", files);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.UploadFile.CopyToAsync(fileStream);
                }

                _context.Add(new ProductPhoto(){
                    ProductID = products.ProductId,
                    FileName = files
                });
               await _context.SaveChangesAsync();
            }
      
            return View(new UploadOneFile());
        }
        // [HttpPost]
        [HttpGet]
        public IActionResult ListPhoto(int id)
        {
          var products = _context.ProductsModels.Where(p=> p.ProductId == id)
            .Include(p => p.Photos)
            .FirstOrDefault();

            if(products == null)
            {
                return Json(
                    new{
                        success = 0,
                        message = "Sản phẩm không tìm thấy",
                    }
                );
            }

         var listphoto =  products.Photos.Select(photo => new{
                id = photo.Id,
                path = "/contents/Products/" + photo.FileName,
            });

            return Json(
                new {
                    success = 1,
                    photos = listphoto
                }
            );

        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = _context.ProductPhotos.Where(p=> p.Id == id).FirstOrDefault();
            if(photo != null)
            {
                _context.Remove(photo);
               
                _context.SaveChanges();

               var fileName = "/contents/Products/" + photo.FileName;
                System.IO.File.Delete(fileName);
            }

            return Ok();

        }

        [HttpPost]
        public IActionResult PhotoDelete(int id)
        {
            var photo = _context.ProductPhotos.Where(p=> p.Id == id).FirstOrDefault();
            if (photo != null)
            {
                string ExitingFile = Path.Combine(_env.WebRootPath, "UploadImage/Products", photo.FileName);
                System.IO.File.Delete(ExitingFile);
            }
           
            return Ok("");
        }


        [Route("/test-file11/")]
        public IActionResult TestFile() 
        {
            return View();
        }


    }
}
