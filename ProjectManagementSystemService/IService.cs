using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public interface IService<T,Dto> where T : class where Dto : class
    {
         Task Add(Dto dto);
         Task Update(Dto dto);
         Task Remove(Dto dto);
         Task<List<T>> GetAll();
         Task<T> Get(int id);
         Task<T> Get(Expression<Func<T,bool>> expression);    
         Task<List<T>> Filter(Expression<Func<T,bool>> expression);

    }
}
