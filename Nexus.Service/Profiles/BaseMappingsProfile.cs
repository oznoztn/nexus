using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;

namespace Nexus.Service.Profiles
{
    public class BaseMappingsProfile : Profile
    {
        public BaseMappingsProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CourseDto, Course>();

            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();

            CreateMap<BookCategory, BookCategoryDto>();
            CreateMap<BookCategoryDto, BookCategory>();

            CreateMap<CourseCategory, CourseCategoryDto>();
            CreateMap<CourseCategoryDto, CourseCategory>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<NoteCategory, NoteCategoryDto>();
            CreateMap<NoteCategoryDto, NoteCategory>();

            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();

            CreateMap<NoteTag, NoteTagDto>();
            CreateMap<NoteTagDto, NoteTag>();

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();

            CreateMap<ProjectPicture, ProjectPictureDto>();
            CreateMap<ProjectPictureDto, ProjectPicture>();

            CreateMap<Note, SlimNoteDto>();
            CreateMap<SlimNoteDto, Note>();
        }
    }
}
