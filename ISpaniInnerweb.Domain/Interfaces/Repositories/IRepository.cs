using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ISpaniInnerweb.Domain.Entities;

namespace ISpaniInnerweb.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T :  HustlersEntity
    {
        void Insert(T item);
        T Get(string id);
        IList<T> Get(int pageSize, int pageNumber, string searchText = null);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionAsNoTracking(Expression<Func<T, bool>> expression);
        IList<T> Get();
        void Update(T item);
        bool Delete(string id);
    }
}
