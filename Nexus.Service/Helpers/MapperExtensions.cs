using System.Collections.Generic;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;

namespace Nexus.Service.Helpers
{
    //public static class MapperExtensions
    //{
    //    public static IEnumerable<NoteCategoryDto> ToDTOs(this IEnumerable<NoteCategory> entities)
    //    {
    //        return Mapper.Map<IEnumerable<NoteCategory>, IEnumerable<NoteCategoryDto>>(entities);
    //    }

    //    public static IEnumerable<NoteCategory> ToEntities(this IEnumerable<NoteCategoryDto> dtos)
    //    {
    //        return Mapper.Map<IEnumerable<NoteCategoryDto>, IEnumerable<NoteCategory>>(dtos);
    //    }

    //    public static NoteCategoryDto ToDTO(this NoteCategory entity)
    //    {
    //        return Mapper.Map<NoteCategory, NoteCategoryDto>(entity);
    //    }

    //    public static NoteCategory ToEntity(this NoteCategoryDto dto)
    //    {
    //        return Mapper.Map<NoteCategoryDto, NoteCategory>(dto);
    //    }


    //    public static IEnumerable<NoteDto> ToDTOs(this IEnumerable<Note> entities)
    //    {
    //        return Mapper.Map<IEnumerable<Note>, IEnumerable<NoteDto>>(entities);
    //    }

    //    public static IEnumerable<Note> ToEntities(this IEnumerable<NoteDto> dtos)
    //    {
    //        return Mapper.Map<IEnumerable<NoteDto>, IEnumerable<Note>>(dtos);
    //    }

    //    public static NoteDto ToDTO(this Note entity)
    //    {
    //        return Mapper.Map<Note, NoteDto>(entity);
    //    }

    //    public static Note ToEntity(this NoteDto dto)
    //    {
    //        return Mapper.Map<NoteDto, Note>(dto);
    //    }


    //    public static IEnumerable<BookDto> ToDTOs(this IEnumerable<Book> entities)
    //    {
    //        return Mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(entities);
    //    }

    //    public static IEnumerable<Book> ToEntities(this IEnumerable<BookDto> dtos)
    //    {
    //        return Mapper.Map<IEnumerable<BookDto>, IEnumerable<Book>>(dtos);
    //    }

    //    public static BookDto ToDTO(this Book entity)
    //    {
    //        return Mapper.Map<Book, BookDto>(entity);
    //    }

    //    public static BookDto ToDTO(this Book entity, int? noteCategoryId, string noteCategorySlug, bool categoryVisibility)
    //    {
    //        var bookDto = Mapper.Map<Book, BookDto>(entity);
    //        bookDto.CategoryId = noteCategoryId;
    //        bookDto.CategorySlug = noteCategorySlug;
    //        bookDto.IsCategoryVisible = categoryVisibility;
    //        return bookDto;
    //    }
    
    //    public static Book ToEntity(this BookDto dto)
    //    {
    //        return Mapper.Map<BookDto, Book>(dto);
    //    }
        
    //    public static IEnumerable<CategoryDto> ToDTOs(this IEnumerable<Category> entities)
    //    {
    //        return Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(entities);
    //    }

    //    public static IEnumerable<Category> ToEntities(this IEnumerable<CategoryDto> dtos)
    //    {
    //        return Mapper.Map<IEnumerable<CategoryDto>, IEnumerable<Category>>(dtos);
    //    }

    //    public static CategoryDto ToDTO(this Category entity)
    //    {
    //        return Mapper.Map<Category, CategoryDto>(entity);
    //    }

    //    public static Category ToEntity(this CategoryDto dto)
    //    {
    //        return Mapper.Map<CategoryDto, Category>(dto);
    //    }

    //    public static IEnumerable<CourseDto> ToDTOs(this IEnumerable<Course> entities)
    //    {
    //        return Mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(entities);
    //    }

    //    public static IEnumerable<Course> ToEntities(this IEnumerable<CourseDto> dtos)
    //    {
    //        return Mapper.Map<IEnumerable<CourseDto>, IEnumerable<Course>>(dtos);
    //    }

    //    public static CourseDto ToDTO(this Course entity)
    //    {
    //        return Mapper.Map<Course, CourseDto>(entity);
    //    }

    //    public static CourseDto ToDTO(this Course entity, int? noteCategoryId, string noteCategorySlug, bool categoryVisibility)
    //    {
    //        var bookDto = Mapper.Map<Course, CourseDto>(entity);
    //        bookDto.CategoryId = noteCategoryId;
    //        bookDto.CategorySlug = noteCategorySlug;
    //        bookDto.IsCategoryVisible = categoryVisibility;
    //        return bookDto;
    //    }

    //    public static Course ToEntity(this CourseDto dto)
    //    {
    //        return Mapper.Map<CourseDto, Course>(dto);
    //    }


    //    public static IEnumerable<NoteTagDto> ToDTOs(this IEnumerable<NoteTag> entities)
    //    {
    //        return Mapper.Map<IEnumerable<NoteTag>, IEnumerable<NoteTagDto>>(entities);
    //    }

    //    public static IEnumerable<NoteTag> ToEntities(this IEnumerable<NoteTagDto> dtos)
    //    {
    //        return Mapper.Map<IEnumerable<NoteTagDto>, IEnumerable<NoteTag>>(dtos);
    //    }

    //    public static NoteTagDto ToDTO(this NoteTag entity)
    //    {
    //        return Mapper.Map<NoteTag, NoteTagDto>(entity);
    //    }

    //    public static NoteTag ToEntity(this NoteTagDto dto)
    //    {
    //        return Mapper.Map<NoteTagDto, NoteTag>(dto);
    //    }
    //}
}
