using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebsite.Models;
using SimpleWebsite.Models.Repositories;

namespace SimpleWebsite.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                    .HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Comment>()
                    .HasOne(c => c.Article)
                    .WithMany(a => a.Comments)
                    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                    .HasOne(c => c.User)
                    .WithMany(u => u.Comments)
                    .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }

        public static async Task Seed(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            await CreateAdminAccount(serviceProvider, configuration);
            await CreateRoles(serviceProvider, configuration);
            await SeedDatabase(serviceProvider);
        }

        private static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = configuration.GetSection("Data:AdminUser:Roles").Get<string[]>();
            foreach (string role in roles)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async static Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string[] roles = configuration.GetSection("Data:AdminUser:Roles").Get<string[]>();

            if (await userManager.FindByNameAsync(username) == null)
            {
                foreach (string role in roles)
                {
                    if (await roleManager.FindByNameAsync(role) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                User user = new User
                {
                    UserName = username,
                    Email = email
                };

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    foreach (string role in roles)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }

        private async static Task SeedDatabase(IServiceProvider serviceProvider)
        {
            ApplicationDbContext context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            UserManager<User> userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            User user = await userManager.FindByNameAsync("Admin");

            if (!context.Articles.Any() && user != null)
            {
                Article article = new Article
                {
                    Header = "Мы открылись!",
                    Text = "Мы открылись!",
                    PublishDate = DateTime.Now,
                    Comments = new List<Comment>
                    {
                        new Comment
                        {
                            User = user,
                            PublishTime = DateTime.Now,
                            Text = "Мы открылись"
                        }
                    }
                };

                context.Articles.Add(article);
                context.SaveChanges();
            }
        }
    }
}