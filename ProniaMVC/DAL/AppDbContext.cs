using Microsoft.EntityFrameworkCore;
using ProniaMVC.Models;
using System.Data.Common;

namespace ProniaMVC.DAL
{
    public class AppDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"server=ertrzl\SQLEXPRESS;database=ProniaMVC;trusted_connection=true;integrated security=true;trustservercertificate=true;");
        }

        public DbSet<Slide>Slides { get; set; }
    }
}
