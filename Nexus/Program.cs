using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using FluentValidation.Internal;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Identity.Data;
using Nexus.Identity.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Nexus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            SetupDirectories(host.Services.GetService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>());
            SetupAdminAccount(host);
            host.Run();
        }

        private static void SetupDirectories(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            string noteImagesDir = Path.Combine(hostingEnvironment.WebRootPath, "images", "notes");
            if (!Directory.Exists(noteImagesDir))
                Directory.CreateDirectory(noteImagesDir);

            string projectImagesDir = Path.Combine(hostingEnvironment.WebRootPath, "images", "projects");
            if (!Directory.Exists(projectImagesDir))
                Directory.CreateDirectory(projectImagesDir);
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private static void SetupAdminAccount(IWebHost host)
        {
            using (IServiceScope serviceScope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                IServiceProvider serviceProvider = serviceScope.ServiceProvider;

                UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

                string name = configuration["Data:AdminUserInfo:Name"];
                string username = configuration["Data:AdminUserInfo:UserName"];
                string email = configuration["Data:AdminUserInfo:Email"];
                string password = configuration["Data:AdminUserInfo:Password"];
                string role = configuration["Data:AdminUserInfo:Role"];

                if (roleManager.RoleExistsAsync(role).Result == false)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();

                    ApplicationUser adminApplicationUser = new ApplicationUser
                    {
                        Name = name,
                        UserName = username,
                        Email = email,
                        EmailConfirmed = true
                    };

                    IdentityResult result = userManager.CreateAsync(adminApplicationUser, password).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminApplicationUser, role).Wait();
                    }
                }
            }

        }
    }
}
