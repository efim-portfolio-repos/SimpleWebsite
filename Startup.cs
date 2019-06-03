using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SimpleWebsite.Models;
using SimpleWebsite.Models.Repositories;

namespace SimpleWebsite
{
    public class Startup
    {

        private IConfiguration _configuration;
        private IHostingEnvironment _environment;
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsDevelopment())
            {
                services.Configure<SecurityStampValidatorOptions>(options =>
                {
                    options.ValidationInterval = TimeSpan.Zero;
                });
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_configuration["Data:SimpleWebsite:ConnectionString"]));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<IRepository<Product>, EfBaseRepository<Product>>();
            services.AddTransient<IRepository<Category>, EfBaseRepository<Category>>();
            services.AddTransient<IRepository<Article>, EfBaseRepository<Article>>();
            services.AddTransient<IRepository<Comment>, EfBaseRepository<Comment>>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(route =>
            {
                route.MapRoute(
                    name: "adminArea",
                    template: "{area:exists}/{controller}/{action=Index}/{id?}"
                );
                route.MapRoute(
                    name: "blogs",
                    template: "Blog/List/Page{page=1}",
                    defaults: new {
                        controller = "Blog",
                        action = "List"
                    });
                route.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var scoppedFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scoppedFactory.CreateScope())
            {
                ApplicationDbContext.Seed(scope.ServiceProvider, _configuration).Wait();
            }
        }
    }
}
