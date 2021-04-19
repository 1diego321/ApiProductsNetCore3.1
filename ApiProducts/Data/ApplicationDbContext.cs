using ApiProducts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationUserStatus> ApplicationUserStatus { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductStatus> ProductStatus { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}
