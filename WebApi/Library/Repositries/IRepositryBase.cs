using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using WebApi.Models;
namespace WebApi.Models
{
    public interface IRepositryBase<T,TContext> where T:class where TContext : DbContext
    {
        IQueryable<T> FindList(Expression<Func<T,bool>> where);
        IQueryable<T> FindPagedList<S>(Expression<Func<T, bool>> where, Expression<Func<T, S>> order, bool isAsc, int page, int size);
        T Find(Expression<Func<T, bool>> where);
    //    T FindById(int id);
        bool Exist(Expression<Func<T,bool>> any);
        bool Add(T model);
        bool Update(T model);
        bool Delete(T model);
    }
}
