using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public interface IService<T,Dto, UpdateDto> where T : class where Dto : class where UpdateDto : class
    {
        Task Add(Dto dto);
        Task Update(T t, Expression<Func<T, bool>> expression);
        Task Update(Expression<Func<T, bool>> expression, T t);
        Task Update(UpdateDto updateDto, Expression<Func<T, bool>> expression);
        Task Update(UpdateDto updateDto, int id);
        Task Update(UpdateDto updateDto, Guid id);
        Task Remove(int id);
        Task Remove(Guid id);
        Task Remove(Expression<Func<T, bool>> expression);
        Task Remove(Guid guid, int id);
        Task Remove(int id, Guid guid);
        Task Remove(Guid id, Guid guid);
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Get(Guid id);
        Task<T> Get(int id, Guid guid);
        Task<T> Get(Guid guid, int id);
        Task<T> Get(Guid id, Guid guid);
        Task<T> Get(Expression<Func<T, bool>> expression);
        List<T> Where(Expression<Func<T, bool>> expression);
    }
}
