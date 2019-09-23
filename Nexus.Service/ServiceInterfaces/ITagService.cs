using System;
using System.Collections.Generic;
using Nexus.Core;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;

namespace Nexus.Service.ServiceInterfaces
{
    public interface ITagService
    {
        TagDto GetById(int id);
        TagDto GetBySlug(string tag);
        TagDto GetByTitle(string title);
        IEnumerable<TagDto> GetAll();
        IEnumerable<Tuple<TagDto, int>> GetTagsAlongWithUsageInformation();
        IEnumerable<Tuple<TagDto, int>> GetTopNTagsAlongWithUsageInfo(int n, bool includeHiddenTags, bool countInvisibleNotes);
        IEnumerable<TagDto> GetTopNTagsWithAtLeastOneNote(int n, bool includeHiddenTags);
        void Update(TagDto tagDto);
        void Delete(TagDto tagDto);
    }
}