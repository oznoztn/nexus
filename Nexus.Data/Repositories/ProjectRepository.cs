using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;

namespace Nexus.Data.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(NexusContext context) : base(context)
        {

        }

        public override IEnumerable<Project> GetAll()
        {
            var projects = Context.Set<Project>().Include(p => p.ProjectPictures).AsEnumerable();
            return projects;
        }

        public Project GetBySlug(string slug)
        {
            var project = Set.Include(pro => pro.ProjectPictures).FirstOrDefault(pr => pr.Slug == slug);
            return project;
        }

        public IEnumerable<Project> GetAllWithFirstPictureIncluded()
        {
            var query =
                from pr in Context.Set<Project>()
                join pp in Context.Set<ProjectPicture>() on pr.Id equals pp.ProjectId into pictures
                select new Project()
                {
                    Id = pr.Id,
                    Title = pr.Title,
                    Abstract = pr.Abstract,
                    Description = pr.Description,
                    Slug = pr.Slug,
                    URL = pr.URL,
                    IsAvocational = pr.IsAvocational,
                    IsVisible = pr.IsVisible,
                    DateStarted = pr.DateStarted,
                    DateFinished = pr.DateFinished,
                    ProjectPictures = pictures.Any(pp => pp.IsVisible) ? new [] { pictures.Where(pp => pp.IsVisible).OrderBy(pp => pp.DisplayOrder).FirstOrDefault()} : null
                };
                
            
            return query.AsEnumerable();
        }
    }
}
