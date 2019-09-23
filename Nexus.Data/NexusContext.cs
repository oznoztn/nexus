using System.IO;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities;
using Microsoft.Extensions.Configuration;
using ProjectPicture = Nexus.Core.Entities.ProjectPicture;

namespace Nexus.Data
{
    public class NexusContext : DbContext
    {
        public NexusContext()
        {

        }

        public NexusContext(DbContextOptions<NexusContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookCategory> BookCategories { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<NoteCategory> NoteCategories { get; set; }

        public virtual DbSet<NoteTag> NoteTags { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectPicture> ProjectPictures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
                // getting the appsetting.json
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                // defining the database provider
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(builder =>
            {
                builder.ToTable(nameof(Tag));
            });

            modelBuilder.Entity<CourseCategory>(builder =>
            {
                builder.ToTable(nameof(CourseCategory));
            });

            modelBuilder.Entity<Project>(builder =>
            {
                builder.ToTable(nameof(Project));
                builder.Property(p => p.Title).HasMaxLength(250);
                builder.Property(p => p.URL).HasMaxLength(100);                
            });

            modelBuilder.Entity<ProjectPicture>(builder =>
            {
                builder.ToTable(nameof(ProjectPicture));
                builder.Property(pp => pp.Title).HasMaxLength(250);
                builder.Property(pp => pp.Alt).HasMaxLength(250);
                builder.Property(pp => pp.Caption).HasMaxLength(250);
            });

            modelBuilder.Entity<Category>(builder =>
            {
                builder.ToTable(nameof(Category));
                builder.Ignore(c => c.CategoryType);
            });

            modelBuilder.Entity<Course>(builder =>
            {
                builder.ToTable(nameof(Course));
                builder
                    .HasOne(c => c.Category)
                    .WithMany()
                    .HasForeignKey(t => t.CategoryId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Book>(builder =>
            {
                builder.ToTable(nameof(Book));
                builder.Ignore(book => book.ReadingStatus);
                builder
                    .HasOne(book => book.Category)
                    .WithMany()
                    .HasForeignKey(book => book.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<BookCategory>(builder =>
            {
                builder.ToTable(nameof(BookCategory));
                builder
                    .HasOne(bc => bc.Book)
                    .WithMany(b => b.BookCategories)
                    .HasForeignKey(bc => bc.BookId);
                builder
                    .HasOne(bc => bc.Category)
                    .WithMany()
                    .HasForeignKey(bc => bc.CategoryId);
            });

            modelBuilder.Entity<Note>(builder =>
            {
                builder.ToTable(nameof(Note)); 
            });

            modelBuilder.Entity<NoteTag>(builder =>
            {
                builder.ToTable(nameof(NoteTag));
                builder.HasKey(nt => new { nt.NoteId, nt.TagId });
            });

            modelBuilder.Entity<NoteCategory>(builder =>
            {
                builder.ToTable(nameof(NoteCategory));
                builder
                    .HasOne(nc => nc.Note)
                    .WithMany(n => n.NoteCategories)
                    .HasForeignKey(nc => nc.NoteId);
                builder
                    .HasOne(nc => nc.Category)
                    .WithMany()
                    .HasForeignKey(nc => nc.CategoryId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
