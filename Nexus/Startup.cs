using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Nexus.Areas.Admin.Models;
using Nexus.Areas.Admin.Validators;
using Nexus.Core.Entities;
using Nexus.Data;
using Nexus.Data.Interfaces;
using Nexus.Data.Repositories;
using Nexus.Identity.Data;
using Nexus.Identity.Models;
using Nexus.Identity.Services;
using Nexus.Profiles;
using Nexus.Service;
using Nexus.Service.DTOs;
using Nexus.Service.Profiles;
using Nexus.Service.ServiceInterfaces;
using Nexus.Tools;

namespace Nexus
{
    public class NexusEnvironment : INexusEnvironment
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NexusEnvironment(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string NoteImagesRootPath => Path.Combine(_webHostEnvironment.WebRootPath, "images", "notes");

        public string GetNoteImagesRootPath(string noteId)
        {
            return Path.Combine(NoteImagesRootPath, noteId);
        }

        public string GetNoteImageRelativePathForWeb(string noteId, string fileName)
        {
            var path = $"images/notes/{noteId}/{fileName}";
            return path;
        }

        public IEnumerable<string> GetNotePictures(string noteId)
        {
            if(noteId == null)
                throw new ArgumentNullException(nameof(noteId));

            if(string.IsNullOrWhiteSpace(noteId))
                throw new ArgumentException(nameof(noteId));

            string directoryToLookFor = GetNoteImagesRootPath(noteId);
            if (Directory.Exists(directoryToLookFor))
            {
                var files = Directory.GetFiles(directoryToLookFor);
                return files;
            }

            return Array.Empty<string>();
        }
    }

    public interface INexusEnvironment
    {
        string NoteImagesRootPath { get; }
        string GetNoteImagesRootPath(string noteId);
        string GetNoteImageRelativePathForWeb(string noteId, string fileName);
        
    }

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IViewRenderService, ViewRenderService>();

            services.AddSingleton<IMessageProvider, MessageProvider>();

            services.AddScoped<NexusContext>();
            services.AddScoped<DbContext, NexusContext>();

            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<INoteTagService, NoteTagService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectPictureService, ProjectPictureService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteTagRepository, NoteTagRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectPictureRepository, ProjectPictureRepository>();

            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<BookCategory>, Repository<BookCategory>>();
            services.AddScoped<IRepository<CourseCategory>, Repository<CourseCategory>>();
            services.AddScoped<IRepository<NoteCategory>, Repository<NoteCategory>>();
            services.AddScoped<IRepository<Book>, Repository<Book>>();
            services.AddScoped<IRepository<Course>, Repository<Course>>();
            services.AddScoped<IRepository<Note>, Repository<Note>>();
            services.AddScoped<IRepository<Tag>, Repository<Tag>>();
            services.AddScoped<IRepository<NoteTag>, Repository<NoteTag>>();
            services.AddScoped<IRepository<Project>, Repository<Project>>();
            services.AddScoped<IRepository<ProjectPicture>, Repository<ProjectPicture>>();

            services.AddScoped<INexusEnvironment, NexusEnvironment>();

            InitIdentity(services);

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new BaseMappingsProfile());
                config.AddProfile(new NoteProfile());
                config.AddProfile(new ProjectProfile());

                config.AddProfile(new BookViewModelProfile());
                config.AddProfile(new NoteViewModelProfile());
                config.AddProfile(new CourseViewModelProfile());
                config.AddProfile(new ProjectModelProfile());
                config.AddProfile(new CategoryViewModelProfile());

                config.AddProfile(new TagViewModelProfile());
            }, typeof(BaseMappingsProfile), typeof(NoteProfile));

            services.AddTransient<IValidator<TagViewModel>, TagViewModelValidator>();

            /*
            // Maintain property names during serialization. See:
            // https://github.com/aspnet/Announcements/issues/194
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddFluentValidation();
            */

            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new CamelCasePropertyNamesContractResolver();
                })
                .AddFluentValidation();
            
            // Add Kendo UI services to the services container
            services.AddKendo();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
           
            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // For wwwroot directory
            //  Core 3.0: If the app calls UseStaticFiles, place UseStaticFiles before UseRouting.

            app.UseStaticFiles();

            // Add support for node_modules folder
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                RequestPath = new PathString("/node")
            });

            app.UseStatusCodePages();
            app.UseHttpsRedirection();

            app.UseRouting();

            // Core 3.0: If the app uses authentication/authorization features such as AuthorizePage or [Authorize], place the call to UseAuthentication and UseAuthorization: after, UseRouting and UseCors, but before UseEndpoints.
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapRazorPages();
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                routes.MapRoute(
                    name: "category",
                    template: "{controller=category}/{slug}",
                    defaults: new { action = "Category" });

                routes.MapRoute(
                    name: "tags",
                    template: "{controller=tags}/{slug}",
                    defaults: new { action = "Index" });

                routes.MapRoute(
                    name: "archive",
                    template: "{controller=notes}/archive/{year}/{month}",
                    defaults: new { action = "Archive" });

                routes.MapRoute(
                    name: "project",
                    template: "{controller=projects}/details/{slug}",
                    defaults: new {action = "Details"});

                routes.MapRoute(
                    name: "notes",
                    template: "{controller=notes}/{id}/{slug}",
                    defaults: new { action = "Note" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Notes}/{action=Index}/{id?}");
            });
        }

        private void InitIdentity(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings                
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                if (_webHostEnvironment.IsDevelopment())
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                // If the LoginPath isn't set, ASP.NET Core defaults the path to /Account/Login.
                options.LoginPath = "/Account/Login";
                // If the AccessDeniedPath isn't set, ASP.NET Core defaults the path to /Account/AccessDenied.
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
