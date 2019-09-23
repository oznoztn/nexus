using System.Collections.Generic;
using AutoMapper;
using Nexus.Core;
using Nexus.Core.Entities;
using Nexus.Service.Interfaces;

namespace Nexus.Service
{
    public class PagedDtoList<TDto> : List<TDto>, IPagedList<TDto> where TDto : IDto
    {
        public PagedDtoList(IMapper mapper, IPagedList<Note> pagedEntityList)
        {
            PageNumber = pagedEntityList.PageNumber;
            PageSize = pagedEntityList.PageSize;
            TotalItemCount = pagedEntityList.TotalItemCount;
            TotalPages = pagedEntityList.TotalPages;
            HasPreviousPage = pagedEntityList.HasPreviousPage;
            HasNextPage = pagedEntityList.HasNextPage;
            IsFirstPage = pagedEntityList.IsFirstPage;
            IsLastPage = pagedEntityList.IsLastPage;
            
            foreach (var entity in pagedEntityList)
            {
                var dto = mapper.Map<IEntity, TDto>(entity);
                Add(dto);
            }
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalItemCount { get; }
        public int TotalPages { get; }
        public bool IsFirstPage { get; }
        public bool IsLastPage { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
    }
}