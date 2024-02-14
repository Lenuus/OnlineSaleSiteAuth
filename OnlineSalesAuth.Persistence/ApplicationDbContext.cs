using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSalesAuth.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<UserToken>().ToTable("UserToken");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaim");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");

            modelBuilder.Entity<User>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Order>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<OrderDetail>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Product>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<ProductCategory>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Coupon>().Property(p => p.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Coupon>().Property(p => p.Used).HasDefaultValue(false);


            modelBuilder.Entity<ProductCategory>().HasOne(p => p.Product).WithMany(p => p.Categories).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductCategory>().HasOne(p => p.Category).WithMany(p => p.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Image>().HasOne(p => p.Product).WithMany(p => p.Images).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Order>().HasOne(p => p.User).WithMany(p => p.Order).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<OrderDetail>().HasOne(p => p.Order).WithMany(p => p.OrderDetails).HasForeignKey(p => p.OrderId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<OrderDetail>().HasOne(p => p.Product).WithMany(p => p.OrderDetails).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Coupon>().HasOne(p => p.Product).WithMany(p => p.Coupons).HasForeignKey(p => p.ProductId).OnDelete(DeleteBehavior.NoAction);
        }

    }
}
