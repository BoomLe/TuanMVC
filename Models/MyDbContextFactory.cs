using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.Models
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost,1433; Initial Catalog=MyBlog; User ID=SA;Password=Tuan@1234;TrustServerCertificate=True");

        return new MyDbContext(optionsBuilder.Options);
    }
}
    
}