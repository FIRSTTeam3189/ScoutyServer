using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RobotServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using RobotServer.SQLDataObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RobotServer {
    public class Startup {
        private static string secretKey = "mysupersecret_secretkey!123";
        private static SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();


        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services.
            services.AddMvc();

            services.AddDbContext<RoboContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("RoboDatabase"));
            });

            services.AddIdentity<Account, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;

                opt.SignIn.RequireConfirmedEmail = false;
                opt.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<RoboContext>().AddDefaultTokenProviders();

            services.AddIdentityServer().AddTemporarySigningCredential().AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources()).AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients()).AddAspNetIdentity<Account>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<Account> um, ILoggerFactory loggerFactory, RoboContext context) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions()
            {
                Authority = Config.HOST,
                RequireHttpsMetadata = true,
                ApiName = "api"
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseStaticFiles();

            RoboContext.Init(um, context, loggerFactory.CreateLogger("init"));

            app.UseMvcWithDefaultRoute();
        }
    }
}
