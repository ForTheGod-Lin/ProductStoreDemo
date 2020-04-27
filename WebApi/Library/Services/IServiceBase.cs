using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Repositries;
using System.Linq.Expressions;
namespace WebApi.Services
{
    public interface IServiceBase<T> 
    {
        
        bool Add(T model);
        bool Update(T model);
        bool Delete(T model);
         IQueryable<T> FindList(Expression<Func<T, bool>> where);
         IQueryable<T> FindPagedList<S>(Expression<Func<T, bool>> where, Expression<Func<T, S>> order, int page, int size, bool isAsc );
         T Find(Expression<Func<T, bool>> where);
         bool Exist(Expression<Func<T, bool>> any);
       
    }
}
