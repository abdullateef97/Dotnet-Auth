using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Identity.Core;


namespace TestIdentity
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
            services.AddMvc();

            var connectionString = "Server=localhost;Database=TestIdentity.ExtendIdentityUser;" +
                                   "Trusted_Connection=True;MultipleActiveResultSets=true;" +
                                   "User ID=SA;password=Abdlatol97;integrated security=false";

            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            services.AddDbContext<TestIdentityUserDbContext>(opt => opt.UseSqlServer(connectionString,
                                                    sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddIdentity<TestIdentityUser, IdentityRole>(options =>
                {
                    options.Tokens.EmailConfirmationTokenProvider = "emailconf";
                })
                .AddEntityFrameworkStores<TestIdentityUserDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfTokenProvider<TestIdentityUser>>("emailconf");
            
            
            
            services.AddScoped<IUserClaimsPrincipalFactory<TestIdentityUser>, TestIdentityUSerClaimsFactory>();
//            services.AddScoped<IUserStore<TestIdentityUser>,
//                UserOnlyStore<TestIdentityUser, TestIdentityUserDbContext>>();

//            services.AddAuthentication("cookies")
//                .AddCookie("cookies", option => option.LoginPath = "/Home/Login");

            //configure lifespan of password reset
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(3));
            
            //configure lifespan of emailConfirmation
            services.Configure<EmailConfTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(3));
            services.ConfigureApplicationCookie(option => option.LoginPath = "/Home/Login");
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
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}