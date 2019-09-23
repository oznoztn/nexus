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
    public class CategoryService : Service<Category, CategoryDto>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRepository<NoteCategory> _noteCategoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository, IRepository<NoteCategory> noteCategoryRepository) : base(categoryRepository, mapper)
        {
            _categoryRepository = categoryRepository;
            _noteCategoryRepository = noteCategoryRepository;
            _mapper = mapper;
        }

        public CategoryDto GetCategoryBySlug(string slug)
        {
            var category = _categoryRepository.GetCategoryBySlug(slug);

            if (category != null)
                return _mapper.Map<CategoryDto>(category);

            return null;
        }

        public IEnumerable<CategoryDto> GetBookCategories()
        {
            var categories = _categoryRepository.GetBookCategories();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return dtos;
        }

        public IEnumerable<CategoryDto> GetCourseCategories()
        {
            var categories = _categoryRepository.GetCourseCategories();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return dtos;
        }

        public IEnumerable<CategoryDto> GetDefaultCategories()
        {
            var categories = _categoryRepository.GetDefaultCategories();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return dtos;
        }

        public IEnumerable<CategoryDto> GetAllOrdered()
        {
            var categories = _categoryRepository.GetAllOrdered(CategoryType.Any);
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return dtos;
        }

        public IEnumerable<CategoryDto> GetDefaultCategoriesOrdered()
        {
            var categories = _categoryRepository.GetDefaultCategoriesOrdered();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return dtos;

        }

        public IEnumerable<CategoryDto> GetCategoriesTree(bool hasDefaultSelectionItem = true)
        {
            List<Category> categories = _categoryRepository.GetAll().ToList();

            if (hasDefaultSelectionItem)
                categories.Insert(0, new Category { Id = 0, ParentId = 0, Title = "[None]" });

            foreach (var category in categories)
            {
                if (category.ParentId != 0)
                {
                    category.Title = categories.First(t => t.Id == category.ParentId).Title + "  >>  " + category.Title;
                }
            }

            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);
        }

        public int GetNextDisplayOrder()
        {
            return _categoryRepository.GetNextDisplayOrder();
        }

        public IEnumerable<Tuple<CategoryDto, int>> GetCategoryNoteCountPairs()
        {
            IEnumerable<Tuple<Category, int>> entityPairs = 
                _categoryRepository.GetCategoryNoteCountPairs();

            IEnumerable<Tuple<CategoryDto, int>> result = 
                entityPairs.Select(tuple => new Tuple<CategoryDto, int>(_mapper.Map<CategoryDto>(tuple.Item1), tuple.Item2));

            return result;
        }

        public IEnumerable<Tuple<CategoryDto, int>> GetCategoryNoteCountPairs(bool includeHiddenCategories, bool includeHiddenNotes)
        {
            IEnumerable<Tuple<Category, int>> entities = 
                _categoryRepository.GetCategoryNoteCountPairs(includeHiddenCategories, includeHiddenNotes);

            var dtos = entities.Select(tuple => new Tuple<CategoryDto, int>(_mapper.Map<CategoryDto>(tuple.Item1), tuple.Item2));

            return dtos;
        }

        public IEnumerable<CategoryDto> GetInnerCategories(bool includeHiddenCategories)
        {
            var categories = _categoryRepository.GetInnerCategories(includeHiddenCategories);
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return dtos;
        }
    }
}
