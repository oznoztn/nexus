using System.Collections.Generic;
using System.Threading.Tasks;
using Nexus.Core.Entities;
using Nexus.Service.Interfaces;

namespace Nexus.Service.GenericService
{
    public interface IService<TEntity, TDto> where TDto : IDto where TEntity : IEntity
    {
        TDto Get(int id);
        IEnumerable<TDto> GetAll();
        Task<TDto> GetAsync(int id);
        Task<IEnumerable<TDto>> GetAllAsync();

        void Add(TDto dto);
        void Add(IEnumerable<TDto> dtos);
        Task<TDto> AddAsync(TDto dto);
        Task<IEnumerable<TDto>> AddAsync(IEnumerable<TDto> dtos);

        TDto Update(TDto dto);
        Task<TDto> UpdateAsync(TDto dto);
        IEnumerable<TDto> Update(IEnumerable<TDto> dtos);
        Task<IEnumerable<TDto>> UpdateAsync(IEnumerable<TDto> dtos);

        void Delete(int id);
        void Delete(TDto dto);
        void Delete(IEnumerable<TDto> dtos);
        Task DeleteAsync(int id);
        Task DeleteAsync(TDto dto);
        Task DeleteAsync(IEnumerable<TDto> dtos);
    }
}