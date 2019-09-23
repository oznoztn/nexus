using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexus.Core
{
    public interface IPagedList<T> : IList<T>
    {
        int PageNumber { get; }
        int PageSize { get; }
        int TotalItemCount { get; }
        int TotalPages { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }

    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IQueryable<T> sourceSequence, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItemCount = sourceSequence.Count();
            TotalPages = TotalItemCount / pageSize;

            if (TotalItemCount % pageSize != 0)
                TotalPages += 1;

            //TotalPages = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;

            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < TotalPages;

            IsFirstPage = pageNumber == 1;
            IsLastPage = PageNumber == TotalPages;

            var subset = pageNumber == 1
                ? sourceSequence.Take(pageSize)
                : sourceSequence.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            AddRange(subset);
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
