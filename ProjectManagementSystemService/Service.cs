using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystemRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class Service<T, Dto, UpdateDto> : IService<T, Dto, UpdateDto> where T : class where Dto : class where UpdateDto : class
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _dbSet;
        private readonly IMemoryCache _memoryCache;
        public Service(IMapper mapper, AppDbContext appDbContext, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
            _memoryCache = memoryCache;
        }
        public async Task Add(Dto dto)
        {
            _dbSet.Add(_mapper.Map<T>(dto));
            await _appDbContext.SaveChangesAsync();

        }



        public async Task<T> Get(int id)
        {
            T t = await _dbSet.FindAsync(id);
            if (t != null)
            {
                return t;
            }
            return null;

        }
        public async Task<T> Get(int id, Guid guid)
        {
            T? t = await _dbSet.FindAsync(id, guid);
            if (t != null)
            {
                return t;
            }
            return null;

        }
        public async Task<T> Get(Guid id, Guid guid)
        {
            T? t = await _dbSet.FindAsync(id, guid);
            if (t != null)
            {
                return t;
            }
            return null;

        }
        public async Task<T> Get(Guid guid, int id)
        {
            T? t = await _dbSet.FindAsync(guid, id);
            if (t != null)
            {
                return t;
            }
            return null;

        }
        public async Task<T> Get(Guid id)
        {
            T t = await _dbSet.FindAsync(id);
            if (t != null)
            {
                return t;
            }
            return null;

        }

        public async Task<List<T>> GetAll(string cacheKey)
        {
            if (_memoryCache.TryGetValue(cacheKey, out List<T> entities) == false)
            {
                entities = await _dbSet.ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                    SlidingExpiration = TimeSpan.FromMinutes(2)

                };
                _memoryCache.Set(cacheKey, entities, cacheEntryOptions);
            }
            return await _dbSet.ToListAsync();
        }

        public async Task Update(T t, Expression<Func<T, bool>> expression)
        {

            var existingEntity = await Get(expression);
            _dbSet.Update(existingEntity);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task Update(Expression<Func<T, bool>> expression, T t)
        {

            await Update(t, expression);
        }
        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            T t = await _dbSet.FirstOrDefaultAsync(expression);
            if (t != null)
            {
                return t;
            }
            return null;
        }

        public async Task Remove(int id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _appDbContext.SaveChangesAsync();

            }
        }

        public async Task Remove(Expression<Func<T, bool>> expression)
        {
            var entity = await Get(expression);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _appDbContext.SaveChangesAsync();

            }
        }

        public async Task Update(UpdateDto updateDto, int id)
        {
            var existingEntity = await Get(id);
            if (existingEntity != null)
            {
                _mapper.Map(updateDto, existingEntity);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Kayıt bulunamadı");
            }

        }
        public async Task Update(UpdateDto updateDto, Guid id)
        {
            var existingEntity = await Get(id);
            if (existingEntity != null)
            {
                _mapper.Map(updateDto, existingEntity);
                await _appDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Kayıt bulunamadı");
            }

        }

        public async Task Update(UpdateDto updateDto, Expression<Func<T, bool>> expression)
        {
            var existingEntity = await Get(expression);
            _mapper.Map(updateDto, existingEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Remove(Guid id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _appDbContext.SaveChangesAsync();

            }
        }
        public async Task Remove(Guid guid, int id)
        {
            var entity = await Get(id, guid);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _appDbContext.SaveChangesAsync();
            }
        }
        public async Task Remove(int id, Guid guid)
        {
            var entity = await Get(guid, id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public List<T> Where(Expression<Func<T, bool>> expression)
        {
            List<T> result = _dbSet.Where(expression).ToList();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public async Task Remove(Guid id, Guid guid)
        {
            var entity = await Get(id, guid);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
