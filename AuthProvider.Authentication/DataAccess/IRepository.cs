using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.DataAccess
{
    public interface IRepository<T> where T : IEntity
    {
        T Get(string id);
        Task<T> GetAsync(string id);

        IList<T> Find(Expression<Func<T, bool>> filter);
        Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter);
        IList<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size);
        Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter, int pageIndex, int size);
        IList<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size);
        Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size);
        IList<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);
        Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);
        IList<T> FindAll();
        Task<IList<T>> FindAllAsync();
        IList<T> FindAll(int pageIndex, int size);
        Task<IList<T>> FindAllAsync(int pageIndex, int size);
        IList<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size);
        Task<IList<T>> FindAllAsync(Expression<Func<T, object>> order, int pageIndex, int size);
        IList<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);
        Task<IList<T>> FindAllAsync(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);

        bool Any(Expression<Func<T, bool>> filter);

        T Last();
        Task<T> LastAsync();
        T Last(Expression<Func<T, bool>> filter);
        Task<T> LastAsync(Expression<Func<T, bool>> filter);
        T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);
        Task<T> LastAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);
        T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);
        Task<T> LastAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);

        T First();
        Task<T> FirstAsync();
        T First(Expression<Func<T, bool>> filter);
        Task<T> FirstAsync(Expression<Func<T, bool>> filter);
        T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);
        Task<T> FirstAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);
        T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);
        Task<T> FirstAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);

        void Update(T entity);
        void Update(IList<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateAsync(IList<T> entities);

        //bool Replace<TField>(T entity, Expression<Func<T, TField>> field, TField value);
        //Task<bool> ReplaceAsync<TField>(T entity, Expression<Func<T, TField>> field, TField value);
        //bool Replace(string id, params UpdateDefinition<T>[] updates);
        //Task<bool> ReplaceAsync(string id, params UpdateDefinition<T>[] updates);
        //bool Replace(T entity, params UpdateDefinition<T>[] updates);
        //Task<bool> ReplaceAsync(T entity, params UpdateDefinition<T>[] updates);
        //bool Replace<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value);
        //Task<bool> ReplaceAsync<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value);
        //bool Replace(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates);
        //Task<bool> ReplaceAsync(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates);
        //bool Replace(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates);
        //Task<bool> ReplaceAsync(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates);

        void Delete(T entity);
        Task DeleteAsync(T entity);
        void Delete(string id);
        Task DeleteAsync(string id);
        void Delete(Expression<Func<T, bool>> filter);
        Task DeleteAsync(Expression<Func<T, bool>> filter);

        void Insert(T entity);
        Task InsertAsync(T entity);
        void Insert(IList<T> entities);
        Task InsertAsync(IList<T> entities);
    }
}
