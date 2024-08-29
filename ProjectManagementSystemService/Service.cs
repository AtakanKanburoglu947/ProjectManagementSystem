using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class Service<T, Dto> : IService<T, Dto> where T : class where Dto : class
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
            return await _dbSet.FindAsync(id); 
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Update(T t)
        {
            _dbSet.Update(_mapper.Map<T>(t));
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<T>> Filter(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync();    
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public async Task Remove(int id)
        {
            _dbSet.Remove(await _dbSet.FindAsync(id));
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Remove(Expression<Func<T, bool>> expression)
        {
            _dbSet.Remove(await _dbSet.FirstOrDefaultAsync(expression));
            await _appDbContext.SaveChangesAsync();
        }
    }
}
