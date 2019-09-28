using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Identity.Models;

namespace Nexus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostBuilder webHost = CreateHostBuilder(args);
            IHost host = webHost.Build();

            SetupDirectories(host.Services.GetService<IWebHostEnvironment>());
            SetupAdminAccount(host);

            host.Run();
        }

        private static void SetupDirectories(IWebHostEnvironment hostingEnvironment)
        {
            string noteImagesDir = Path.Combine(hostingEnvironment.WebRootPath, "images", "notes");
            if (!Directory.Exists(noteImagesDir))
                Directory.CreateDirectory(noteImagesDir);

            string projectImagesDir = Path.Combine(hostingEnvironment.WebRootPath, "images", "projects");
            if (!Directory.Exists(projectImagesDir))
                Directory.CreateDirectory(projectImagesDir);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void SetupAdminAccount(IHost host)
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
