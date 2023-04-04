using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Blog.Models;
using App.Areas.Contacts.Models;
using App.Areas.Identity.Models;
using App.Areas.Post.Models;
using App.Areas.Product.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace App.Models
{
    public class MyDbContext : IdentityDbContext<MyUserRole>
    {
        public DbSet<ContactModel> contactModel{set;get;}
        public DbSet<Category> categoryModel{set;get;}

         public DbSet<PostModel> Posts {set; get;}
         public DbSet<PostCategoryModel> PostCategories {set; get;}

         public DbSet<CategoryProduct> CategoriesProduct{set;get;}

         public DbSet<ProductsModel> ProductsModels{set;get;}

         public DbSet<ProductInCategory> ProductsInCategory{set;get;}

         public DbSet<ProductPhoto> ProductPhotos{set;get;}
        public MyDbContext(DbContextOptions<MyDbContext>options) : base(options)
        {
            
        }
     
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tables = entity.GetTableName();
                if(tables.StartsWith("AspNet"))
                {
                   entity.SetTableName(tables.Substring(6));
                }
                
            }

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex( p => p.Slug);
                
                
            });

            modelBuilder.Entity<PostCategoryModel>(entity =>
            {
                entity.HasKey( p => new{ p.PostID, p.CategoryID});
            });

            modelBuilder.Entity<PostModel>(entity =>
            {
                entity.HasIndex( s => s.Slug).IsUnique();

            });


// Product
             modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasIndex( p => p.Slug);
                
                
            });

            modelBuilder.Entity<ProductInCategory>(entity =>
            {
                entity.HasKey( p => new{ p.ProductID, p.CategoryID});
            });

            modelBuilder.Entity<ProductsModel>(entity =>
            {
                entity.HasIndex( s => s.Slug).IsUnique();

            });

            
           
          
           

        }

        public DbSet<App.Areas.Blog.Models.Category> Category { get; set; } = default!;
        // public DbSet<App.Areas.Product.Models.CategoryProduct> CategoryProduct { get; set; } = default!;
        
    }
}