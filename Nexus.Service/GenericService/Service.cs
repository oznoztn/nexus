using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Data;
using Nexus.Service.Interfaces;

namespace Nexus.Service.GenericService
{
    public class Service<TE, TD> : IService<TE, TD> 
        where TE : class, IEntity 
        where TD : class, IDto
    {
        private readonly IRepository<TE> _genericRepository;
        private readonly IMapper _mapper;

        public Service(IRepository<TE> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        private async Task<TE> AddAsyncPrivate(TE entity)
        {
            _genericRepository.Add(entity);
            await _genericRepository.UnitOfWork.SaveChangesAsync();

            return entity;
        }

        private async Task<IEnumerable<TE>> AddAsyncPrivate(IEnumerable<TE> entities)
        {
            var es = entities.ToList();
            _genericRepository.Add(es);
            await _genericRepository.UnitOfWork.SaveChangesAsync();
            return es;
        }

        private async Task<TE> UpdateAsyncPrivate(TE entity)
        {
            _genericRepository.Update(entity);
            await _genericRepository.UnitOfWork.SaveChangesAsync();

            return entity;
        }

        private async Task<IEnumerable<TE>> UpdateAsyncPrivate(IEnumerable<TE> entities)
        {
            var es = entities.ToList();
            _genericRepository.Update(es);
            await _genericRepository.UnitOfWork.SaveChangesAsync();

            return es;
        }

        private async Task DeleteAsyncPrivate(TE entity)
        {
            _genericRepository.Delete(entity);
            await _genericRepository.UnitOfWork.SaveChangesAsync();
        }

        private async Task DeleteAsyncPrivate(IEnumerable<TE> entities)
        {
            _genericRepository.Delete(entities);
            await _genericRepository.UnitOfWork.SaveChangesAsync();
        }

        public virtual TD Get(int id)
        {
            TE entity = _genericRepository.Get(id);
            TD dto = _mapper.Map<TD>(entity);
            return dto;
        }

        public virtual async Task<TD> GetAsync(int id)
        {
            var entity = await _genericRepository.GetAsync(id);
            TD dto = _mapper.Map<TD>(entity);
            return dto;
        }

        public virtual IEnumerable<TD> GetAll()
        {
            IEnumerable<TE> entities = _genericRepository.GetAll();

            var dtos = _mapper.Map<IEnumerable<TE>, IEnumerable<TD>>(entities);

            return dtos;
        }

        public virtual async Task<IEnumerable<TD>> GetAllAsync()
        {
            var entities = await _genericRepository.GetAllAsync();

            var returnList = new List<TD>();
            foreach (var entity in entities)
            {
                var dto = _mapper.Map<TE, TD>(entity);
                returnList.Add(dto);
            }
            return returnList;
        }

        public virtual void Add(TD dto)
        {
            var entity = _mapper.Map<TE>(dto);

            _genericRepository.Add(entity);
            _genericRepository.UnitOfWork.SaveChanges();

            _mapper.Map(entity, dto);
        }

        public virtual async Task<TD> AddAsync(TD dto)
        {
            var entity = _mapper.Map<TE>(dto);

            entity = await this.AddAsyncPrivate(entity);

            return _mapper.Map<TD>(entity);
        }

        public virtual void Add(IEnumerable<TD> dtos)
        {
            var entities = _mapper.Map<IEnumerable<TE>>(dtos).ToList();

            _genericRepository.Add(entities);
            _genericRepository.UnitOfWork.SaveChanges();

            _mapper.Map(entities, dtos);
        }

        public virtual async Task<IEnumerable<TD>> AddAsync(IEnumerable<TD> dtos)
        {
            var entities = _mapper.Map<IEnumerable<TE>>(dtos);

            await this.AddAsyncPrivate(entities);

            return _mapper.Map<IEnumerable<TD>>(entities);
        }

        public virtual TD Update(TD dto)
        {
            var entity = _mapper.Map<TE>(dto);

            _genericRepository.Update(entity);
            _genericRepository.UnitOfWork.SaveChanges();

            return _mapper.Map<TD>(entity);
        }

        public virtual async Task<TD> UpdateAsync(TD dto)
        {
            var entity = _mapper.Map<TE>(dto);

            await this.UpdateAsyncPrivate(entity);

            return _mapper.Map<TD>(entity);
        }

        public virtual IEnumerable<TD> Update(IEnumerable<TD> dtos)
        {
            var entities = _mapper.Map<IEnumerable<TE>>(dtos);

            _genericRepository.Update(entities);
            _genericRepository.UnitOfWork.SaveChanges();

            return _mapper.Map<IEnumerable<TD>>(entities);
        }
        
        public virtual async Task<IEnumerable<TD>> UpdateAsync(IEnumerable<TD> dtos)
        {
            var entities = _mapper.Map<IEnumerable<TE>>(dtos);

            await this.UpdateAsyncPrivate(entities);

            return _mapper.Map<IEnumerable<TD>>(entities);
        }

        public virtual void Delete(int id)
        {
            _genericRepository.Delete(id);
            _genericRepository.UnitOfWork.SaveChanges();
        }

        public virtual async Task DeleteAsync(int id)
        {
            _genericRepository.Delete(id);
            await _genericRepository.UnitOfWork.SaveChangesAsync();
        }

        public virtual void Delete(TD dto)
        {
            TE entity = _mapper.Map<TE>(dto);
            _genericRepository.Delete(entity);
            _genericRepository.UnitOfWork.SaveChanges();
        }

        public virtual async Task DeleteAsync(TD dto)
        {
            TE entity = _mapper.Map<TE>(dto);
            await this.DeleteAsyncPrivate(entity);
        }

        public virtual void Delete(IEnumerable<TD> dtos)
        {
            var entities = _mapper.Map<IEnumerable<TE>>(dtos);
            _genericRepository.Delete(entities);
            _genericRepository.UnitOfWork.SaveChanges();
        }

        public virtual async Task DeleteAsync(IEnumerable<TD> dtos)
        {
            var entities = _mapper.Map<IEnumerable<TE>>(dtos);
            await this.DeleteAsyncPrivate(entities);
        }
    }
}