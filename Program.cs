using System.Net;
using App.Areas.Contacts.Models;
using App.Areas.Identity.Models;
using App.Areas.Post.Models;
using App.Areas.Product.Service;
using App.ErrorPages;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("MyWebiste"));
    }
);

builder.Services.AddDefaultIdentity<MyUserRole>(options => options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();

// builder.WebHost.UseUrls("http://localhost:5001","https://localhost:5555");
// builder.Services.AddHttpsRedirection(option =>{
//     option.HttpsPort = 5555;
// });

//ADD secsion
builder.Services.AddDistributedMemoryCache();  // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
builder.Services.AddSession(cfg => {           // Đăng ký dịch vụ Session
    cfg.Cookie.Name = "Hoangtuan";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
    cfg.IdleTimeout = new TimeSpan(0,30, 0);   // Thời gian tồn tại của Session
});


// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = false;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

});

builder.Services.AddSingleton<IEmailSender, SendMailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/khong-duoc-truy-cap.html";
});

// ADD dịch vụ thứ 3 Google
builder.Services.AddAuthentication().AddGoogle(options =>
{
    var addGoogle = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = addGoogle["ClientId"];
    options.ClientSecret = addGoogle["ClientSecret"];
    options.CallbackPath ="/dang-nhap-tu-google/";
});
//add Cart
builder.Services.AddTransient<CartService>();
//ADd Service Food:
builder.Services.AddSingleton<FoodService>();
//Add service Contact
builder.Services.AddSingleton<ContactModel>();
// Add services to the container.

//Add Error fix Token Sysytem
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityError>();
builder.Services.AddSingleton<PostBaseModel, PostModel>();
builder.Services.AddControllersWithViews();
// Razor
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    
    app.UseHsts();
}
app.UseCookiePolicy();
app.ErrorsWebsite();
app.UseStaticFiles();
app.UseHttpsRedirection();

// controll thư mục lưu
app.UseStaticFiles(new StaticFileOptions(){
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(),"UploadImage" )
    ),
    RequestPath = "/contents"
});
app.UseSession();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(enpoint =>
{
    enpoint.MapGet("/hi", async context =>
    {
      await  context.Response.WriteAsync($" Today : {DateTime.Now}");
    });
    enpoint.MapRazorPages();
});

app.MapAreaControllerRoute(
    name : "PostCategorys",
    pattern : "{controller=PostControllers}/{action=Index}/{id?}",
    areaName : "Blog"
);

// app.MapAreaControllerRoute(
//     name : "BlogCategory",
//     pattern : "{controller}/{action=Index}/{id?}",
//     areaName : "Blogs"
// );

app.MapAreaControllerRoute(
    name : "topfood",
    pattern : "{controller}/{action=Index}/{id?}",
    areaName : "Foods"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
