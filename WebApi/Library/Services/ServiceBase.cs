using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using WebApi.Repositries;
using System.Linq.Expressions;
using System.Data.Entity;
namespace WebApi.Services
{
    public class ServiceBase<T,TContext>:IServiceBase<T> where T:class where TContext:DbContext
    {
        public IRepositryBase<T,TContext> Repositry { get; set; }
        public ServiceBase(IRepositryBase<T, TContext> r)
        {
            Repositry = r;
        }
        public T Add(T model)
        {
            return Repositry.Add(model);
        }

        public bool Update(T model)
        {
            return Repositry.Update(model);
        }

        public bool Delete(T model)
        {
            return Repositry.Delete(model);
        }
        public IQueryable<T> FindList(Expression<Func<T, bool>> where)
        {
            return Repositry.FindList(where);
        }

        public IQueryable<T> FindPagedList<S>(Expression<Func<T, bool>> where, Expression<Func<T, S>> order, int page, int size, bool isAsc = true)
        {
            return Repositry.FindPagedList(where, order, isAsc, page, size);
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return Repositry.Find(where);
        }

        public bool Exist(Expression<Func<T, bool>> any)
        {
            return Repositry.Exist(any);
        }
    }
}