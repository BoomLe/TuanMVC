using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Post.Models;
using App.Areas.Product.Models;
using App.Areas.Product.Service;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Areas.Product.Controllers
{
   [Area("Product")]
    public class ViewProductControllers : Controller
    {
        private readonly ILogger<ViewProductControllers> _logger;
        private readonly MyDbContext _context;
        private readonly CartService _cartService;

        public ViewProductControllers(ILogger<ViewProductControllers> logger,MyDbContext context,  CartService cartService)
        {
            _logger = logger;
            _context = context;
            _cartService = cartService;
        }

        [Route("/product/{categoryslug?}")]
        public IActionResult Index(string categoryslug,[FromQuery(Name ="p")] int currentPage, int pagesize)
        {
            var categories = GetCategories();
            ViewBag.categories = categories;
            ViewBag.categoryslug = categoryslug;

            CategoryProduct category = null;
            if(!string.IsNullOrEmpty(categoryslug))
            {
                category = _context.CategoriesProduct.Where( p=> p.Slug == categoryslug)
                .Include(p => p.CategoryChildren)
                .FirstOrDefault();

                if(category == null)
                {
                    return NotFound("Không tìm thấy");
                }
            }

            var products = _context.ProductsModels.Include(p => p.Author)
            .Include(p =>p.Photos)
            .Include(p=> p.ProductInCategories)
            .ThenInclude(p => p.Category)
            .AsQueryable();

          products =  products.OrderByDescending(p => p.DateUpdated);

            if(category != null)
            {
                var ids = new List<int>();
                category.ChildCategoryIDs(null, ids);
                ids.Add(category.Id);

                products = products.Where(p => p.ProductInCategories.Where(pc => ids.Contains(pc.CategoryID)).Any());
            }

              int totalPosts =  products.Count();
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

             var postsInPage = products.Skip((currentPage - 1) * pagesize)
            .Take(pagesize);
           


            ViewBag.PagingModel = pagingModel;
            ViewBag.totalPosts = totalPosts;

            ViewBag.category = category;
            return View(postsInPage.ToList());
        }

        [Route("/product/{postslug}.html")]
        public IActionResult Detail(string postslug)
        {
            var categories =GetCategories();
            ViewBag.categories = categories;

            var products = _context.ProductsModels.Where(p=> p.Slug == postslug)
            .Include(p=> p.Author)
            .Include(p=> p.Photos)
            .Include(p => p.ProductInCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefault();

            if(products == null)
            {
                return NotFound("không tìm thấy bài");
            }
            
            CategoryProduct category = products.ProductInCategories.FirstOrDefault()?.Category;
            ViewBag.category =category;

            var otherPost = _context.ProductsModels.Where(p => p.ProductInCategories.Any(p => p.Category.Id == category.Id))
            .Where(p => p.ProductId != products.ProductId)
            .OrderByDescending(p => p.DateUpdated)
            .Take(2);

            ViewBag.otherPost = otherPost;
            return View(products);
        }

        private List<CategoryProduct> GetCategories()
        {
            var categories = _context.CategoriesProduct
            .Include(p => p.CategoryChildren)
            .AsEnumerable()
            .Where(p => p.ParentCategory == null)
            .ToList();
            return categories;
        }

    [Route ("addcart/{productid:int}", Name = "addcart")]
    public IActionResult AddToCart ([FromRoute] int productid) {

         var product = _context.ProductsModels
        .Where (p => p.ProductId == productid)
        .FirstOrDefault ();
        if (product == null)
        return NotFound ("Không có sản phẩm");

    // Xử lý đưa vào Cart ...
        var cart = _cartService.GetCartItems();
        var cartitem = cart.Find (p => p.product.ProductId == productid);
        if (cartitem != null) {
        // Đã tồn tại, tăng thêm 1
        cartitem.quantity++;
    } 
    else {
        //  Thêm mới
        cart.Add (new CartItem () { quantity = 1, product = product });
    }

    // Lưu cart vào Session
        _cartService.SaveCartSession(cart);
    // Chuyển đến trang hiện thị Cart
        return RedirectToAction (nameof (Cart));
    }

    // Hiện thị giỏ hàng
     [Route ("/cart", Name = "cart")]
         public IActionResult Cart () 
        {
         return View (_cartService.GetCartItems());
        }

    /// xóa item trong cart
           [Route ("/removecart/{productid:int}", Name = "removecart")]
    public IActionResult RemoveCart ([FromRoute] int productid) {
           var cart =_cartService.GetCartItems ();
              var cartitem = cart.Find (p => p.product.ProductId == productid);
                if (cartitem != null) {
                   //       Đã tồn tại, tăng thêm 1
                  cart.Remove(cartitem);
             }

            _cartService.SaveCartSession (cart);
              return RedirectToAction (nameof (Cart));
               }

               [Route ("/updatecart", Name = "updatecart")]
         [HttpPost]
          public IActionResult UpdateCart ([FromForm] int productid, [FromForm] int quantity)
          {
                   // Cập nhật Cart thay đổi số lượng quantity ...
                 var cart = _cartService.GetCartItems ();
               var cartitem = cart.Find (p => p.product.ProductId == productid);
              if (cartitem != null) {
             // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
                    }
               _cartService.SaveCartSession (cart);
                  // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
                   return Ok();
                  }
            [Route("/checkout")]
            public IActionResult Checkout()
            {
                var cart = _cartService.GetCartItems();
                //
                _cartService.ClearCart();
                return Content("Đã xác nhận dơn hàng");
            }



        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }



    }
}