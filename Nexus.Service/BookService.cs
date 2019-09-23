using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Data;
using Nexus.Data.Interfaces;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;
using Nexus.Service.Helpers;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Service
{
    public class BookService : Service<Book, BookDto>, IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRepository<BookCategory> _bookCategoryRepository;

        public BookService(IMapper mapper, IBookRepository bookRepository, ICategoryRepository categoryRepository, IRepository<BookCategory> bookCategoryRepository) : base(bookRepository, mapper)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _bookCategoryRepository = bookCategoryRepository;
        }

        public override IEnumerable<BookDto> GetAll()
        {
            var query =
                from b in _bookRepository.AllInclude(book => book.BookCategories)
                join c in _categoryRepository.GetAll() on b.CategoryId equals c.Id into cc
                from t in cc.DefaultIfEmpty()
                select this.BookToDto(b, t?.Id ?? 0, t?.Slug ?? "", t?.IsVisible ?? false);
                //select new BookDto
                //{
                //    Id = b.Id,
                //    NoteCategoryId = t?.Id ?? 0,
                //    NoteCategorySlug = t?.Slug ?? "",
                //    ReadingStatusId = b.ReadingStatusId,
                //    YearFinished = b.YearFinished,
                //    PublicationYear = b.PublicationYear,
                //    Title = b.Title,
                //    Author = b.Author,
                //    DisplayOrder = b.DisplayOrder,
                //    Edition = b.Edition,
                //    Pages = b.Pages,
                //    CoverImage = b.CoverImage,
                //    IsVisible = b.IsVisible
                //};

            List<BookDto> result = query.ToList();

            return result;
        }

        private BookDto BookToDto(Book book, int? noteCategoryId, string noteCategorySlug, bool categoryVisibility)
        {
            var bookDto = _mapper.Map<Book, BookDto>(book);
            bookDto.CategoryId = noteCategoryId;
            bookDto.CategorySlug = noteCategorySlug;
            bookDto.IsCategoryVisible = categoryVisibility;
            return bookDto;
        }


        public ImageUploadStatus UpdateCoverImage(byte[] data, int bookId)
        {
            try
            {
                _bookRepository.UpdateCoverImage(data, bookId);               
                return new ImageUploadStatus { IsSuccessful = true };
            }
            catch (Exception e)
            {
                return new ImageUploadStatus { Error = e.Message, IsSuccessful = false};
            }
        }

        public List<BookGroup> GetBookGroups()
        {
            var books = this.GetAll();
            var bookCategories = _bookCategoryRepository.GetAll();
            var categories = _categoryRepository.GetAll().Where(c => c.CategoryTypeId == (int)CategoryType.Default);

            var query =
                from b in books
                join bc in bookCategories on b.Id equals bc.BookId
                join c in categories.Where(c => c.CategoryTypeId == (int)CategoryType.Default) on bc.CategoryId equals c.Id
                group b by c.Title into bookGroup
                select new BookGroup()
                {
                    CategoryTitle = bookGroup.Key,
                    Books = bookGroup.ToList()
                };

            var result = query.ToList();

            return result;
        }

        public int GetNextDisplayOrder()
        {
            return _bookRepository.GetNextDisplayOrder();
        }
    }
}
