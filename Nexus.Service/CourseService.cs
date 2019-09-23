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
    public class CourseService : Service<Course, CourseDto>, ICourseService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CourseCategory> _courseCategoryRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CourseService(IMapper mapper, IRepository<CourseCategory> courseCategoryRepository, ICourseRepository courseRepository, ICategoryRepository categoryRepository) : base(courseRepository, mapper)
        {
            _mapper = mapper;
            _courseCategoryRepository = courseCategoryRepository;
            _courseRepository = courseRepository;
            _categoryRepository = categoryRepository;
        }

        public int GetNextDisplayOrder()
        {
            return _courseRepository.GetNextDisplayOrder();
        }

        public override IEnumerable<CourseDto> GetAll()
        {

            var query =
                from course in _courseRepository.AllInclude(course => course.CourseCategories)
                join category in _categoryRepository.GetAll() on course.CategoryId equals category.Id into cc
                from c in cc.DefaultIfEmpty()
                select this.CourseToDto(course, c?.Id ?? 0, c?.Slug ?? "", c?.IsVisible ?? false);

            List<CourseDto> result = query.ToList();
            return result;
        }

        private CourseDto CourseToDto(Course course, int? noteCategoryId, string noteCategorySlug, bool categoryVisibility)
        {
            var courseDto = _mapper.Map<CourseDto>(course);
            courseDto.CategoryId = noteCategoryId;
            courseDto.CategorySlug = noteCategorySlug;
            courseDto.IsCategoryVisible = categoryVisibility;
            return courseDto;
        }

        public List<CourseGroup> GetCourseGroups()
        {
            //var categories = _categoryRepository.GetAll().Where(c => c.CategoryTypeId == (int)CategoryType.Default);
            //var courses = GetAll();

            //var query =
            //    from cat in categories
            //    join co in courses on cat.Id equals co.CategoryId
            //    group co by new { cat.Title, cat.Id, cat.Slug } into grup
            //    select new CourseGroup
            //    {
            //        CategoryId = grup.Key.Id,
            //        CategoryTitle = grup.Key.Title,
            //        Courses = grup.ToList()
            //    };
            //return query.ToList();

            /*
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
            */

            var courses = this.GetAll();
            var courseCategories = _courseCategoryRepository.GetAll();
            var categories = _categoryRepository.GetAll().Where(c => c.CategoryTypeId == (int)CategoryType.Default);

            var query =
                from b in courses
                join bc in courseCategories on b.Id equals bc.CourseId
                join c in categories.Where(c => c.CategoryTypeId == (int)CategoryType.Default) on bc.CategoryId equals c.Id
                group b by c.Title into courseGroup
                select new CourseGroup
                {
                    CategoryTitle = courseGroup.Key,
                    Courses = courseGroup.ToList()
                };

            var result = query.ToList();
            return result;
        }
    }
}