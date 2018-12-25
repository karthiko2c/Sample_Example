
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SubQuip.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SubQuip.Common.Extensions;
using SubQuip.Common;
using MongoDB.Bson.Serialization;

namespace SubQuip.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private static string connectionString;
        private static IMongoClient server;// = new MongoClient(connectionString);
        private string collectionName;
        private IMongoDatabase db;

        public Repository(IConfiguration configuration, string collection)
        {
            collectionName = collection;
            connectionString = configuration.GetConnectionString("SubQuipConnection");
            server = new MongoClient(connectionString);
            db = server.GetDatabase(MongoUrl.Create(connectionString).DatabaseName);
        }

        protected IMongoCollection<T> Collection
        {
            get
            {
                return db.GetCollection<T>(collectionName);
            }
            set
            {
                Collection = value;
            }
        }

        public IMongoQueryable<T> Query
        {
            get
            {
                return Collection.AsQueryable<T>();
            }
            set
            {
                Query = value;
            }
        }

        public T GetOne(Expression<Func<T, bool>> expression)
        {
            return Collection.Find(expression).SingleOrDefault();
        }

        public T FindOneAndUpdate(Expression<Func<T, bool>> expression, UpdateDefinition<T> update, FindOneAndUpdateOptions<T> option)
        {
            return Collection.FindOneAndUpdate(expression, update, option);
        }

        public void UpdateOne(Expression<Func<T, bool>> expression, UpdateDefinition<T> update)
        {
            Collection.UpdateOne(expression, update);
        }

        public void DeleteOne(Expression<Func<T, bool>> expression)
        {
            Collection.DeleteOne(expression);
        }

        public void DeleteMany()
        {
            var filter = Builders<T>.Filter.Empty;
            Collection.DeleteMany(filter);
        }

        public void InsertMany(IEnumerable<T> items)
        {
            Collection.InsertMany(items);
        }

        public void InsertOne(T item)
        {
            Collection.InsertOne(item);
        }

        public IQueryable<T> Page(IQueryable<T> source, int page, int pageSize)
        {
            return PagingExtensions.Page(source, page, pageSize);
        }

        public IQueryable<T> Filter(Expression<Func<T, bool>> expression)
        {
            return Query.Where(expression);
        }

        public IQueryable<T> Sort(IQueryable<T> source, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), collectionName);

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            switch (sortDirection.ToLower())
            {
                case "asc":
                    return source.OrderBy<T, object>(sortExpression);
                default:
                    return source.OrderByDescending<T, object>(sortExpression);
            }
        }

        public T GetChildDocument(Expression<Func<T, bool>> filterExpression, FieldDefinition<T> includeProperties, FieldDefinition<T> excludeProperties = null)
        {
            var filter = Builders<T>.Filter.Where(filterExpression);
            var projection = Builders<T>.Projection.Include(includeProperties);
            if (excludeProperties != null)
            {
                projection.Exclude(excludeProperties);
            }
            var result = Collection.Find<T>(filter).Project(projection).FirstOrDefault();
            return BsonSerializer.Deserialize<T>(result);
        }

        public T GetMultipleChildDocument(Expression<Func<T, bool>> filterExpression, List<FieldDefinition<T>> includeProperties, FieldDefinition<T> excludeProperties = null)
        {
            var filter = Builders<T>.Filter.Where(filterExpression);
            var projection = Builders<T>.Projection.Include(includeProperties.First()); ;
            foreach (var field in includeProperties.Skip(1))
            {
                projection = projection.Include(field);
            }

            if (excludeProperties != null)
            {
                projection.Exclude(excludeProperties);
            }
            var result = Collection.Find<T>(filter).Project(projection).FirstOrDefault();
            return BsonSerializer.Deserialize<T>(result);
        }
    }
}
