using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Student.Web.Models;
using System;
using System.Security.Claims;

namespace Student.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<StudentDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 2;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<StudentDbContext>()
            .AddDefaultTokenProviders();


            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddAuthentication().AddGoogle(fo =>
            {
                fo.ClientId = Configuration["Authentication:Google:ClientId"];
                fo.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                fo.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                fo.ClaimActions.Clear();
                fo.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                fo.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                fo.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                fo.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                fo.ClaimActions.MapJsonKey("urn:google:profile", "link");
                fo.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                fo.ClaimActions.MapJsonKey("urn:google:image", "picture");
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
