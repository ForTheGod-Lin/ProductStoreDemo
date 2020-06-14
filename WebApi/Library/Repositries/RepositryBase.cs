using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WebApi.Models;
namespace WebApi.Models
{
    public class RepositryBase<T,TContext> : IRepositryBase<T,TContext> where T: class where TContext:DbContext
    {
        public TContext Context { get; set; }
        public RepositryBase(TContext context)
        {
            Context = context;
        }
        
        public bool Add(T model)
        {
            Context.Entry<T>(model).State = EntityState.Added;
           return Context.SaveChanges()>0;
            
        }

        public bool Delete(T model)
        {
             Context.Entry<T>(model).State = EntityState.Deleted;
            return Context.SaveChanges() > 0;
            
        }

        public bool Update(T model)
        {
            Context.Entry<T>(model).State = EntityState.Modified;
            return Context.SaveChanges() > 0;
        }

        public IQueryable<T> FindList(Expression<Func<T, bool>> where)
        {
            if (where == null) return Context.Set<T>();
                return Context.Set<T>().Where(where); 
        }

        public IQueryable<T> FindPagedList<S>(Expression<Func<T, bool>> where, Expression<Func<T, S>> order, bool isAsc, int page, int size)
        {
            if (isAsc)
                return Context.Set<T>().Where(where).OrderBy(order).Skip(page-1).Take(size);
            else return Context.Set<T>().Where(where).OrderByDescending(order).Skip(page - 1).Take(size);
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return Context.Set<T>().Where(where).FirstOrDefault();
        }
        public T FindById(params object[] ids)
        {
            return Context.Set<T>().Find(ids);
        }
        public bool Exist(Expression<Func<T, bool>> any)
        {
            return Context.Set<T>().Any(any);
        }

  
    }
}
