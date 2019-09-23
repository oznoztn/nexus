using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;
using Remotion.Linq.Clauses;

namespace Nexus.Data.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(NexusContext context) : base(context)
        {
        }

        public override Course Get(int id)
        {
            Course course = Set.AsNoTracking().Include(c => c.CourseCategories).SingleOrDefault(c => c.Id == id);

            return course;
        }

        public override void Update(Course course)
        {
            var currentCategories = Context.Set<CourseCategory>().Where(t => t.CourseId == course.Id).ToList();
            var incomingCategories = course.CourseCategories.ToList();

            // TODO: learn how to implement IEqualityComparer<T>
            var currentIds = currentCategories.Select(t => t.CategoryId).ToList();
            var incomingIds = incomingCategories.Select(t => t.CategoryId).ToList();

            var addedIds = incomingIds.Except(currentIds).ToArray();
            var removedIds = currentIds.Except(incomingIds).ToArray();

            Context.Set<CourseCategory>().AddRange(addedIds.Select(addedCatId => new CourseCategory { CategoryId = addedCatId, CourseId = course.Id }));
            foreach (var removedId in removedIds)
            {
                var removed = Context.Set<CourseCategory>().First(t => t.CategoryId == removedId && t.CourseId == course.Id);
                Context.Set<CourseCategory>().Remove(removed);
            }

            course.CourseCategories = null;

            Set.Update(course);
            //Context.SaveChanges();
        }

        public IEnumerable<Course> GetCoursesByCategory(int categoryId)
        {
            var query = Set.Where(c => c.CategoryId == categoryId);
            
            return query.AsNoTracking().AsEnumerable();
        }

        public IEnumerable<Course> GetCoursesByCategories(int[] categoryIds)
        {
            var query = Context.Set<Course>().Where(c => categoryIds.Any(t => t == c.Id));

            return query.AsEnumerable();
        }

        //public void GetCoursesGroupedByCategories()
        //{
        //    var query =
        //        from ca in _context.Set<Category>().Where(c => c.CategoryType == CategoryType.Default).OrderBy(c => c.DisplayOrder)
        //        join cc in _context.Set<CourseCategory>() on ca.Id equals cc.CategoryId
        //        join co in _context.Set<Course>() on cc.CourseId equals co.Id
        //        group co.Title by new { cc.CategoryId, ca.DisplayOrder, CategoryTitle = ca.Title } into gr
        //        orderby gr.Key.DisplayOrder
        //        select new
        //        {
        //            Category = gr.Key.CategoryTitle,
        //            Courses = gr.ToList()
        //        };
            
        //    var result = query.ToList();
        //}

        public int GetNextDisplayOrder()
        {
            if (!Set.Any())
                return 1;
            
            return Set.Max(e => e.DisplayOrder) + 1;
        }
    }
}