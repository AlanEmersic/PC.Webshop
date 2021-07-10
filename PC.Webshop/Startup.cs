using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PC.Webshop.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PC.Webshop.Model;
using Microsoft.AspNetCore.Identity;

namespace PC.Webshop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();  

            services.AddDbContext<WebshopDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("WebshopDbContext")));

            services.AddIdentity<Customer, IdentityRole>().AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultUI().AddDefaultTokenProviders().AddEntityFrameworkStores<WebshopDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "proizvodi",
                    pattern: "proizvodi",
                    defaults: new { controller = "Product", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
