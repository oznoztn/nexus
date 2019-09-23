using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Core.Entities;

namespace Nexus.Service.Queries
{
    public static class TagQueries
    {
        public static IQueryable<Tag> Paged(this IQueryable<Tag> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
