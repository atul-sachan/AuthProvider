using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.DataAccess
{
    public class MongoRepository<T> : MongoRepositoryBase, IRepository<T>
        where T : IEntity
    {
        public MongoRepository(IConfiguration configuration)
            : base(configuration)
        {

        }

        public MongoRepository(IConfiguration configuration, string connectionId)
            : base(configuration, connectionId)
        {

        }

        public IMongoCollection<T> Collection
        {
            get
            {
                return GetCollection<T>();
            }
        }

        public FilterDefinitionBuilder<T> Filter => Builders<T>.Filter;
        public ProjectionDefinitionBuilder<T> Project => Builders<T>.Projection;

        private IFindFluent<T, T> Query(Expression<Func<T, bool>> filter)
        {
            return Collection.Find(filter);
        }

        private IFindFluent<T, T> Query()
        {
            return Collection.Find(Filter.Empty);
        }

        #region Get
        public virtual T Get(string id)
        {
            return Retry(() =>
            {
                return Find(i => i.Id == id).FirstOrDefault();
            });
        }

        public virtual async Task<T> GetAsync(string id)
        {
            return await Retry(async () =>
            {
                return await FirstAsync(i => i.Id == id);
            });
        }
        #endregion

        #region Find

        public virtual IList<T> Find(Expression<Func<T, bool>> filter)
        {
            return Query(filter).ToList();
        }

        public virtual async Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            return await Query(filter).ToListAsync();
        }

        public IList<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size)
        {
            return Find(filter, i => i.Id, pageIndex, size);
        }

        public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter, int pageIndex, int size)
        {
            return await FindAsync(filter, i => i.Id, pageIndex, size);
        }

        public IList<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return Find(filter, order, pageIndex, size, true);
        }

        public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return await FindAsync(filter, order, pageIndex, size, true);
        }

        public virtual IList<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return Retry(() =>
            {
                var query = Query(filter).Skip(pageIndex * size).Limit(size);
                return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToList();
            });
        }

        public virtual async Task<IList<T>> FindAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return await Retry(async () =>
            {
                var query = Query(filter).Skip(pageIndex * size).Limit(size);
                return await (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToListAsync();
            });
        }
        #endregion

        #region FindAll

        public virtual IList<T> FindAll()
        {
            return Retry(() => Query().ToList());
        }

        public virtual async Task<IList<T>> FindAllAsync()
        {
            return await Retry(async () => await Query().ToListAsync());
        }

        public IList<T> FindAll(int pageIndex, int size)
        {
            return FindAll(i => i.Id, pageIndex, size);
        }

        public async Task<IList<T>> FindAllAsync(int pageIndex, int size)
        {
            return await FindAllAsync(i => i.Id, pageIndex, size);
        }

        public IList<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return FindAll(order, pageIndex, size, true);
        }

        public async Task<IList<T>> FindAllAsync(Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return await FindAllAsync(order, pageIndex, size, true);
        }

        public virtual IList<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return Retry(() =>
            {
                var query = Query().Skip(pageIndex * size).Limit(size);
                return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToList();
            });
        }

        public virtual async Task<IList<T>> FindAllAsync(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            return await Retry(async () =>
            {
                var query = Query().Skip(pageIndex * size).Limit(size);
                return await (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToListAsync();
            });
        }

        #endregion FindAll

        #region Any

        public bool Any(Expression<Func<T, bool>> filter)
        {
            return Retry(() => Collection.AsQueryable<T>().Any(filter));
        }

        #endregion Simplicity

        #region Last

        public T Last()
        {
            return FindAll(i => i.Id, 0, 1, true).FirstOrDefault();
        }

        public async Task<T> LastAsync()
        {
            return await Retry(async () =>
            {
                var query = Query().Skip(0 * 1).Limit(1);
                return await query.SortByDescending(i => i.Id).FirstOrDefaultAsync();
            });
        }

        public T Last(Expression<Func<T, bool>> filter)
        {
            return Last(filter, i => i.Id);
        }

        public async Task<T> LastAsync(Expression<Func<T, bool>> filter)
        {
            return await Task.FromException<T>(new Exception());
        }

        public T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return Last(filter, order, false);
        }

        public async Task<T> LastAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return await LastAsync(filter, order, false);
        }

        public T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return First(filter, order, !isDescending);
        }

        public async Task<T> LastAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return await FirstAsync(filter, order, !isDescending);
        }

        #endregion Last

        #region First

        public T First()
        {
            return FindAll(i => i.Id, 0, 1, false).FirstOrDefault();
        }

        public async Task<T> FirstAsync()
        {
            return await Retry(async () =>
            {
                var query = Query().Skip(0 * 1).Limit(1);
                return await query.SortBy(i => i.Id).FirstOrDefaultAsync();
            });

        }

        public T First(Expression<Func<T, bool>> filter)
        {
            return First(filter, i => i.Id);
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> filter)
        {
            return await FirstAsync(filter, i => i.Id);
        }

        public T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return First(filter, order, false);
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order)
        {
            return await FirstAsync(filter, order, false);
        }

        public T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return Find(filter, order, 0, 1, isDescending).SingleOrDefault();
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending)
        {
            return await Retry(async () =>
            {
                var query = Query(filter).Skip(0 * 1).Limit(1);
                return await (isDescending ? query.SortByDescending(order) : query.SortBy(order)).SingleOrDefaultAsync();
            });
        }
        #endregion First

        #region Update
        public virtual void Update(T entity)
        {
            Retry(() =>
            {
                return Collection.ReplaceOne(i => i.Id == entity.Id, entity);
            });
        }

        public void Update(IList<T> entities)
        {
            foreach (T entity in entities)
            {
                Update(entity);
            }
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await Retry(async () =>
            {
                return await Collection.ReplaceOneAsync(i => i.Id == entity.Id, entity);
            });
        }

        public async Task UpdateAsync(IList<T> entities)
        {
            foreach (T entity in entities)
            {
                await UpdateAsync(entity);
            }
        }
        #endregion

        #region Replace

        #endregion

        #region Delete

        public void Delete(T entity)
        {
            Delete(entity.Id);
        }

        public async Task DeleteAsync(T entity)
        {
            await DeleteAsync(entity.Id);
        }

        public virtual void Delete(string id)
        {
            Retry(() =>
            {
                return Collection.DeleteOne(i => i.Id == id);
            });
        }

        public virtual async Task DeleteAsync(string id)
        {
            await Retry(async () =>
            {
                return await Collection.DeleteOneAsync(i => i.Id == id);
            });
        }

        public void Delete(Expression<Func<T, bool>> filter)
        {
            Retry(() =>
            {
                return Collection.DeleteMany(filter);
            });
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> filter)
        {
            await Retry(async () =>
            {
                return await Collection.DeleteManyAsync(filter);
            });
        }
        #endregion

        #region Insert
        public virtual void Insert(T entity)
        {
            Retry(() =>
            {
                Collection.InsertOne(entity);
                return true;
            });
        }

        public virtual async Task InsertAsync(T entity)
        {
            await Retry(async () =>
            {
                await Collection.InsertOneAsync(entity);
                return true;
            });
        }
        public virtual void Insert(IList<T> entities)
        {
            Retry(() =>
            {
                Collection.InsertMany(entities);
                return true;
            });
        }

        public virtual async Task InsertAsync(IList<T> entities)
        {
            await Retry(async () =>
            {
                await Collection.InsertManyAsync(entities);
                return true;
            });
        }
        #endregion

        #region RetryPolicy

        protected virtual TResult Retry<TResult>(Func<TResult> action)
        {
            return RetryPolicy
                .Handle<MongoConnectionException>(i => i.InnerException.GetType() == typeof(IOException))
                .Retry(3)
                .Execute(action);
        }

        #endregion
    }
}
