using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Blog.Models;
using App.Areas.Identity.Models;
using App.Data;
using App.Models;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Areas.Post.Models;
using App.Areas.Product.Models;

namespace App.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/admin-databse/[action]")]
    public class DbManagerController : Controller
    {
        private readonly MyDbContext _context;
        private readonly ILogger<DbManagerController> _logger;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<MyUserRole> _userManager;
        public DbManagerController(MyDbContext context, ILogger<DbManagerController> logger,
        RoleManager<IdentityRole> roleManager,
        UserManager<MyUserRole> userManager)
        {
            _logger = logger;
            _context = context;
            _roleManager =roleManager;
            _userManager =userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
         public IActionResult Delete()
        {
            return View();
        }
        [TempData]
        public string StatusMessage{set;get;}
        [HttpPost]
        public async Task<IActionResult> DeleteAsync()
        {
            var deleteDb = await _context.Database.EnsureDeletedAsync();
            StatusMessage = deleteDb ? "Xóa Database thành công" : "Xóa Database không thành công";
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Migrations()
        {
            await _context.Database.MigrateAsync();
            StatusMessage = "Tạo dữ liệu Databse thành công";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AdminDataBase()
        {
            var roleNamez = typeof(RoleName).GetFields().ToList();

            foreach (var r in roleNamez)
            {
                var RoleNames = (string)r.GetRawConstantValue();

                var FoundRole = await _roleManager.FindByNameAsync(RoleNames);

                if(FoundRole == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(RoleNames));

                }
            }
               var userAdmin = await _userManager.FindByNameAsync("Admin");
               if(userAdmin == null)
               {
                userAdmin = new MyUserRole()
                {
                    UserName = "Admin",
                    Email = "gomesle1998@icloud.com",
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(userAdmin, "Tuan@123");
                await _userManager.AddToRoleAsync(userAdmin, RoleName.Administrator);
               }

               FakePostCategory();
               FakeProductCategory();
               return RedirectToAction("Index"); 
            
        }

         private async void FakeProductCategory()
        {
            _context.CategoriesProduct.RemoveRange(_context.CategoriesProduct.Where(c => c.Content.Contains("[FakeData]")));
            // _context.ProductsModels.RemoveRange(_context.ProductsModels.Where(p => p.Content.Contains("[FakeData]")));

             _context.SaveChanges();
            // Randomizer.Seed = new Random(8675309);
            var FakePost = new Faker<CategoryProduct>();
            int cm = 1;
            FakePost.RuleFor(c => c.Title, fk =>$"Nhom SP {cm++}" + fk.Lorem.Sentence(1,2).Trim('.'));
            FakePost.RuleFor( c=> c.Content, fk => fk.Lorem.Sentences(5) + "[FakeData]");
            FakePost.RuleFor(c => c.Slug, fk=> fk.Lorem.Slug());

            var cate1 = FakePost.Generate();
                 var cate11 = FakePost.Generate();
                  var cate111 = FakePost.Generate();
            
            var cate2 = FakePost.Generate();
                 var cate22 = FakePost.Generate();
                 var cate222 = FakePost.Generate();


           cate11.ParentCategory = cate1;
           cate111.ParentCategory =cate1;

           cate22.ParentCategory =cate2;
           cate222.ParentCategory = cate2;
           var CategoryProduct = new CategoryProduct[]{cate1 ,cate2, cate11, cate22, cate111, cate222};
            _context.CategoriesProduct.AddRange(CategoryProduct);


        //         var RCategory = new Random();
        //    int bv = 1;

        //    var Fakeuser = _userManager.GetUserAsync(this.User).Result;
        //    var FakeCategoryProduct = new Faker<ProductsModel>();
        //    FakeCategoryProduct.RuleFor( p => p.AuthorId, m => Fakeuser.Id );
        //    FakeCategoryProduct.RuleFor( p => p.Content, m => m.Commerce.ProductDescription() + "[FakeDate]");
        //    FakeCategoryProduct.RuleFor( p => p.DateCreated , m => m.Date.Between(new DateTime(2023,1,1), new DateTime(2023,3,24)));
        //    FakeCategoryProduct.RuleFor(p => p.Description, m => m.Lorem.Sentences(3));
        //    FakeCategoryProduct.RuleFor( p => p.Published, m => true);
        //    FakeCategoryProduct.RuleFor( p=> p.Slug, m=> m.Lorem.Slug());
        //    FakeCategoryProduct.RuleFor( p=> p.Title, m => $"San Pham{bv++}" + m.Commerce.ProductName());
        //    FakeCategoryProduct.RuleFor(p=> p.Price, m=> int.Parse(m.Commerce.Price(500, 1000, 0)));

        //    List<ProductsModel> Products = new List<ProductsModel>();
        //    List<ProductInCategory> productInCategory = new List<ProductInCategory>();

        //    for(int i = 0; i < 40; i++ )
        //    {
        //     var Product = FakeCategoryProduct.Generate();
        //     Product.DateUpdated = Product.DateCreated;
        //     Products.Add(Product);
        //     productInCategory.Add(new ProductInCategory()
        //     {
        //         Product = Product,
        //         Category = CategoryProduct[RCategory.Next(5)]
        //     });

        //    }
          
        //    _context.AddRange(Products);
        //     _context.AddRange(productInCategory);




            _context.SaveChanges();

        
        }

        private async void FakePostCategory()
        {
            _context.categoryModel.RemoveRange(_context.categoryModel.Where(c => c.Content.Contains("[FakeData]")));
            // _context.Posts.RemoveRange(_context.Posts.Where(p => p.Content.Contains("[FakeData]")));
             _context.SaveChanges();

            // Randomizer.Seed = new Random(8675309);
            var FakePost = new Faker<Category>();
            int cm = 1;
            FakePost.RuleFor(c => c.Title, fk =>$"CM {cm++}" + fk.Lorem.Sentence(1,2).Trim('.'));
            FakePost.RuleFor( c=> c.Content, fk => fk.Lorem.Sentences(5) + "[FakeData]");
            FakePost.RuleFor(c => c.Slug, fk=> fk.Lorem.Slug());

            var cate1 = FakePost.Generate();
                 var cate11 = FakePost.Generate();
                  var cate111 = FakePost.Generate();
            
            var cate2 = FakePost.Generate();
                 var cate22 = FakePost.Generate();
                 var cate222 = FakePost.Generate();


           cate11.ParentCategory = cate1;
           cate111.ParentCategory =cate1;

           cate22.ParentCategory =cate2;
           cate222.ParentCategory = cate2;
           var CategoryPost = new Category[]{cate1 ,cate2, cate11, cate22, cate111, cate222};
            _context.categoryModel.AddRange(CategoryPost);
           

        //    var RCategory = new Random();
        //    int bv = 1;

        //    var user = _userManager.GetUserAsync(this.User).Result;
        //    var FakeCategoryPost = new Faker<PostModel>();
        // //    FakeCategoryPost.RuleFor( p => p.AuthorId, user.Id);
        //    FakeCategoryPost.RuleFor( p => p.Content, m => m.Lorem.Paragraphs(7) + "[FakeDate]");
        //    FakeCategoryPost.RuleFor( p => p.DateCreated , m => m.Date.Between(new DateTime(2023,1,1), new DateTime(2023,3,24)));
        //    FakeCategoryPost.RuleFor(p => p.Description, m => m.Lorem.Sentences(3));
        //    FakeCategoryPost.RuleFor( p => p.Published, m => true);
        //    FakeCategoryPost.RuleFor( p=> p.Slug, m=> m.Lorem.Slug());
        //    FakeCategoryPost.RuleFor( p=> p.Title, m => $"Bài{bv++}" + m.Lorem.Sentence(3,4).Trim('.'));

        //    List<PostModel> posts = new List<PostModel>();
        //    List<PostCategoryModel> post_category = new List<PostCategoryModel>();

        //    for(int i = 0; i < 40; i++ )
        //    {
        //     var post = FakeCategoryPost.Generate();
        //     post.DateUpdated = post.DateCreated;
        //     posts.Add(post);
        //     post_category.Add(new PostCategoryModel()
        //     {
        //         Post = post,
        //         Category =CategoryPost[RCategory.Next(5)]
        //     });

        //    }
          
        //    _context.AddRange(posts);
        //     _context.AddRange(post_category);
            _context.SaveChanges();

        }



    }
}