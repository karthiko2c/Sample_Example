using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace SubQuip.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IMongoQueryable<T> Query { get; set; }

        T GetOne(Expression<Func<T, bool>> expression);

        T FindOneAndUpdate(Expression<Func<T, bool>> expression, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> option);

        void UpdateOne(Expression<Func<T, bool>> expression, UpdateDefinition<T> update);

        void DeleteOne(Expression<Func<T, bool>> expression);

        void DeleteMany();

        void InsertMany(IEnumerable<T> items);

        void InsertOne(T item);

        T GetChildDocument(Expression<Func<T, bool>> filterExpression, FieldDefinition<T> includeProperties, FieldDefinition<T> excludeProperties = null);

        T GetMultipleChildDocument(Expression<Func<T, bool>> filterExpression, List<FieldDefinition<T>> includeProperties, FieldDefinition<T> excludeProperties = null);

        IQueryable<T> Page(IQueryable<T> source, int page, int pageSize);

        IQueryable<T> Filter(Expression<Func<T, bool>> expression);

        IQueryable<T> Sort(IQueryable<T> source, string sortBy, string sortDirection);
    }
}
