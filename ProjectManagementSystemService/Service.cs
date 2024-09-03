using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
        public Service(IMapper mapper, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
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
        public async Task<T> Get(Guid id)
        {
            T t = await _dbSet.FindAsync(id);
            if (t != null)
            {
                return t;
            }
            return null;

        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Update(T t, Expression<Func<T,bool>> expression)
        {

            var existingEntity = await Get(expression);
            _dbSet.Update(existingEntity);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<T>> Filter(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();    
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

        public async Task Update(UpdateDto updateDto,int id)
        {
            var existingEntity = await Get(id);
            if (existingEntity != null)
            {
                _mapper.Map(updateDto,existingEntity);
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
    }
}
