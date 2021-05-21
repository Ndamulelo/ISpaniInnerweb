using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ISpaniInnerweb.Infrastructure.Repositories
{
    public  class Repository<T> : IRepository<T> where T :  HustlersEntity
    {
        protected readonly HustlersContext context;

        public Repository(HustlersContext context)
        {
            this.context = context;
        }
        //this.RepositoryContext.Set<T>().Remove(entity);
        public bool Delete(string id)
        {
            var item = context.Set<T>().Find(id);
            if (item != null)
            {
                //this.context.Update(item);
                this.context.Remove(item);
                this.context.SaveChanges();
                return true;
            }
            return false;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.context.Set<T>().Where(expression).AsNoTracking();
        }        
        
        public IQueryable<T> FindByConditionAsNoTracking(Expression<Func<T, bool>> expression)
        {
            return this.context.Set<T>().Where(expression);
        }

        public virtual T Get(string id)
        {
            var item = context.Set<T>().Where(x => x.Id == id && x.IsActive).FirstOrDefault();
            return item;
        }

        public virtual IList<T> Get( int pageSize, int pageNumber, string searchText=null)
        {
            var item = context.Set<T>().Where(x => x.IsActive);
            var results = item.Take(pageSize).Skip(pageSize * pageNumber - 1).ToList();
            return results;
        }

        public virtual IList<T> Get()
        {
            var item = context.Set<T>().Where(x => x.IsActive);
            return item.ToList();
        }

        public  void Insert(T item)
        {
            context.Set<T>().Add(item);
            context.SaveChanges();
        }

        public void Update(T item)
        {
            var results = context.Set<T>().Find(item.Id);

            if (results != null)
            {
                this.context.Update(item);
                this.context.SaveChanges();
            }
        }
    }
}
