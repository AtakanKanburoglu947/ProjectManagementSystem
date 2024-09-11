using Auth.Services;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemMVC.Models;
using ProjectManagementSystemRepository;
using System.Linq;
using System.Linq.Expressions;

namespace ProjectManagementSystemMVC
{
    public static class Pagination<T,V> where T : class where V: class 
    {   
        public static PaginationModel<T,V> Model(int id, Guid userIdentityId,V? data, List<T>? dataset, int count)
        {

            PaginationModel<T, V> paginationModel = new PaginationModel<T, V>();
            if (data != null)
            {
                paginationModel.Data = data;
            }
            if (dataset != null)
            {
                if (id > 0)
                {
                    id *= 4;
                }
                paginationModel.Pagination = new PaginationViewModel() { Count = count};
                paginationModel.Dataset = dataset;


            }
            return paginationModel;
        }
    }
}
