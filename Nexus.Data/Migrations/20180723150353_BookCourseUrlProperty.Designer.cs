﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nexus.Data;

namespace Nexus.Data.Migrations
{
    [DbContext(typeof(NexusContext))]
    [Migration("20180723150353_BookCourseUrlProperty")]
    partial class BookCourseUrlProperty
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Nexus.Core.Entities.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Author");

                    b.Property<int?>("CategoryId");

                    b.Property<byte[]>("CoverImage");

                    b.Property<string>("CoverImageMime");

                    b.Property<int>("DisplayOrder");

                    b.Property<bool>("IsVisible");

                    b.Property<int>("Pages");

                    b.Property<int>("PublicationYear");

                    b.Property<int>("ReadingStatusId");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.Property<int?>("YearFinished");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("Nexus.Core.Entities.BookCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BookId");

                    b.Property<int>("CategoryId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BookCategory");
                });

            modelBuilder.Entity("Nexus.Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryTypeId");

                    b.Property<string>("Description");

                    b.Property<int>("DisplayOrder");

                    b.Property<bool>("IsVisible");

                    b.Property<int>("ParentId");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Nexus.Core.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abstract");

                    b.Property<string>("Author");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("DisplayOrder");

                    b.Property<int>("Duration");

                    b.Property<bool>("IsOnlineCourse");

                    b.Property<bool>("IsVisible");

                    b.Property<string>("Provider");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.Property<int>("YearFinished");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("Nexus.Core.Entities.CourseCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<int>("CourseId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseCategory");
                });

            modelBuilder.Entity("Nexus.Core.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abstract");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("IsVisible");

                    b.Property<DateTime?>("LastUpdateDate");

                    b.Property<string>("Slug");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Nexus.Core.Entities.NoteCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<int>("NoteId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("NoteId");

                    b.ToTable("NoteCategory");
                });

            modelBuilder.Entity("Nexus.Core.Entities.NoteTag", b =>
                {
                    b.Property<int>("NoteId");

                    b.Property<int>("TagId");

                    b.Property<int>("Id");

                    b.HasKey("NoteId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NoteTag");
                });

            modelBuilder.Entity("Nexus.Core.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Abstract");

                    b.Property<DateTime?>("DateFinished");

                    b.Property<DateTime>("DateStarted");

                    b.Property<string>("Description");

                    b.Property<bool>("IsAvocational");

                    b.Property<bool>("IsVisible");

                    b.Property<string>("Slug");

                    b.Property<string>("Title")
                        .HasMaxLength(250);

                    b.Property<string>("URL")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Nexus.Core.Entities.ProjectPicture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Alt")
                        .HasMaxLength(250);

                    b.Property<byte[]>("Binary");

                    b.Property<string>("Caption")
                        .HasMaxLength(250);

                    b.Property<int>("DisplayOrder");

                    b.Property<string>("FileName");

                    b.Property<bool>("IsVisible");

                    b.Property<int>("ProjectId");

                    b.Property<string>("Title")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectPicture");
                });

            modelBuilder.Entity("Nexus.Core.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsHidden");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Nexus.Core.Entities.Book", b =>
                {
                    b.HasOne("Nexus.Core.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Nexus.Core.Entities.BookCategory", b =>
                {
                    b.HasOne("Nexus.Core.Entities.Book", "Book")
                        .WithMany("BookCategories")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nexus.Core.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nexus.Core.Entities.Course", b =>
                {
                    b.HasOne("Nexus.Core.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Nexus.Core.Entities.CourseCategory", b =>
                {
                    b.HasOne("Nexus.Core.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nexus.Core.Entities.Course", "Course")
                        .WithMany("CourseCategories")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nexus.Core.Entities.NoteCategory", b =>
                {
                    b.HasOne("Nexus.Core.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nexus.Core.Entities.Note", "Note")
                        .WithMany("NoteCategories")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nexus.Core.Entities.NoteTag", b =>
                {
                    b.HasOne("Nexus.Core.Entities.Note", "Note")
                        .WithMany("NoteTags")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nexus.Core.Entities.Tag", "Tag")
                        .WithMany("NoteTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nexus.Core.Entities.ProjectPicture", b =>
                {
                    b.HasOne("Nexus.Core.Entities.Project", "Project")
                        .WithMany("ProjectPictures")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
