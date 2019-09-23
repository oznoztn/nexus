using System.Collections.Generic;
using System.Linq;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;

namespace Nexus.Data.Repositories
{
    public class ProjectPictureRepository : Repository<ProjectPicture>, IProjectPictureRepository
    {
        private readonly NexusContext _context;

        public ProjectPictureRepository(NexusContext context) : base(context)
        {
            _context = context;
        }

        public void UpdatePictureInformation(ProjectPicture projectPicture)
        {
            var picture = _context.ProjectPictures.Find(projectPicture.Id);

            picture.FileName = projectPicture.FileName;
            picture.Caption = projectPicture.Caption;
            picture.DisplayOrder = projectPicture.DisplayOrder;
        }

        public IEnumerable<ProjectPicture> GetAllWithoutRawBinary(int projectId)
        {
            var query = 
                GetProjectPicturesQuery(includeRawImageBinary: false)
                    .Where(pp => pp.ProjectId == projectId);

            return query.AsEnumerable();
        }

        private IQueryable<ProjectPicture> GetProjectPicturesQuery(bool includeRawImageBinary)
        {
            var projectPictures =
                Context.Set<ProjectPicture>()
                    .Select(pp => new ProjectPicture
                    {
                        Id = pp.Id,
                        Binary = includeRawImageBinary ? pp.Binary : null,
                        IsVisible = pp.IsVisible,
                        Title = pp.Title,
                        ProjectId = pp.ProjectId,
                        Caption = pp.Caption,
                        Alt = pp.Alt,
                        DisplayOrder = pp.DisplayOrder,
                        FileName = pp.FileName
                    });

            return projectPictures;

        }
    }
}