using log4net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using NxtLvl.Core.Common;

namespace NxtLvl.Azure.Data
{
    /// <summary>
    /// Class that encapsluates the complexity of doing simple interactions with the Azure ComsoDB platform.
    /// </summary>
    /// <typeparam name="TBase">The base type the CosmosEntityStore is configured to work with.</typeparam>
    public class CosmosEntityStore<TBase> : IEntityStore<TBase, Guid>, ILinqQuery
        where TBase : CosmosDocument
    {
        #region fields

        readonly string _authKey, _databaseName, _collectionName;
        readonly Uri _uri;
        readonly ILog _log;

        DocumentClient _client;
        Database _database;
        DocumentCollection _collection;
        bool _initialized;

        #endregion

        #region constructor

        /// <summary>
        /// The constructor for the CosmosEntityStore class specifying the dependencies and information needed about the Azure ComsoDB platform instance to which you intend to interact.
        /// </summary>
        /// <param name="log">log4net's ILog interface which will be used to log class initialization and other events.</param>
        /// <param name="uri">The URI value of your CosmsoDB instance to which you want to interact.  Available in the Azure Portal.</param>
        /// <param name="authKey">The Authentication Key to allow access to your CosmosDB instance.</param>
        /// <param name="databaseName">The name of the database instance inside your CosmosDB instance to which you want to interact.</param>
        /// <param name="collectionName">The name of the collection instance inside your database instance to which you want to interact.</param>
        public CosmosEntityStore(ILog log, string uri, string authKey, string databaseName, string collectionName)
        {
            Validate.ArgumentIsNotNull(log, nameof(log));
            Validate.ArgumentIsNotNullOrEmpty(uri, nameof(uri));
            Validate.ArgumentIsNotNullOrEmpty(authKey, nameof(authKey));
            Validate.ArgumentIsNotNullOrEmpty(databaseName, nameof(databaseName));
            Validate.ArgumentIsNotNullOrEmpty(collectionName, nameof(collectionName));

            log.Info($"The CosmosEntityStore with URI: {uri} for Database: {databaseName} and Collection: {collectionName} has begun construction.");

            _log = log;
            _uri = new Uri(uri);
            _authKey = authKey;
            _databaseName = databaseName;
            _collectionName = collectionName;

            _log.Info($"The CosmosEntityStore with URI:{uri} for Database:{databaseName} and Collection:{collectionName} has finished construction.");
        }

        #endregion

        #region public methods

        /// <summary>
        /// Uses the information provided in the constructor on how to access your CosmsoDB instance, and ensures a connection can be made and that the database and collection exist and are ready to accept data interactions.
        /// </summary>
        /// <remarks>
        /// This method should be called only once per instance.  It is recommended to call this method during the initialization or dependency configuration stage of your application to warm up the underlying components to avoid cold call latency.
        /// </remarks>
        public async Task Initialize()
        {
            if (_initialized)
                return;

            _client = new DocumentClient(_uri, _authKey);
            _database = await _client.CreateDatabaseIfNotExistsAsync(new Database() { Id = _databaseName });
            _collection = await _client.CreateDocumentCollectionIfNotExistsAsync(_database.SelfLink,
                                                                                           new DocumentCollection() { Id = _collectionName },
                                                                                           new RequestOptions() { OfferThroughput = 1000 });
            _initialized = true;
        }

        /// <summary>
        /// Add the provided Entity to the CosmosDB table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to add.</param>
        /// <returns>The added Entity.</returns>
        public async Task<TEntity> AddAsync<TEntity>(TEntity item)
            where TEntity : TBase, new()
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            if (!item.Id.HasValue)
                item.Id = Guid.NewGuid();

            item.SystemType = typeof(TEntity).FullName;

            var result = await _client.CreateDocumentAsync(_collection.SelfLink, item);

            var response = new TEntity();
            response.Hydrate(result.Resource);
            return response;
        }

        /// <summary>
        /// Delete the provided Entity from the CosmosDB table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to delete.</param>
        /// <returns>The deleted Entity.</returns>
        public async Task<TEntity> DeleteAsync<TEntity>(TEntity item)
            where TEntity : TBase, new()
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            var document = await GetDocumentAsync(item.Id.Value);

            if (document == null)
                return null;

            var result = await _client.DeleteDocumentAsync(document.SelfLink);

            var response = new TEntity();
            response.Hydrate(document);
            return response;
        }

        /// <summary>
        /// Get the Entity associated with the provided Id from the CosmosDB table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="id">The Id value of the Entity to get.</param>
        /// <returns>The Entity associated with the provided Id value.</returns>
        public async Task<TEntity> GetAsync<TEntity>(Guid id)
            where TEntity : TBase, new()
        {
            await Initialize();

            var document = await GetDocumentAsync(id);

            if (document == null)
                return null;

            var response = new TEntity();
            response.Hydrate(document);
            return response;
        }

        /// <summary>
        /// Find any Entities that satisfy the conditions provided in the Linq predicate from the CosmosDB table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="predicate">The condition the Entities must satifsy.</param>
        /// <returns>The Entities that satisfy the condition specified.</returns>
        public async Task<IList<TItem>> FindAsync<TItem>(Expression<Func<TItem, bool>> predicate)
        {
            Validate.ArgumentIsNotNull(predicate, nameof(predicate));

            await Initialize();

            return await Task.Run(() => _client.CreateDocumentQuery<TItem>(_collection.DocumentsLink).Where(predicate).ToList());
        }

        /// <summary>
        /// Add the provided Entity to the CosmosDB table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to add.</param>
        /// <returns>The added Entity.</returns>
        public async Task<TEntity> UpdateAsync<TEntity>(TEntity item)
            where TEntity : TBase, new()
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            if (!item.Id.HasValue)
                throw new ArgumentException("The Id property of the item parameters was null.");

            var document = await GetDocumentAsync(item.Id.Value);

            if (document == null)
                throw new Exception($"The record with Id:{item.Id.Value} did not exist and hence cannot be updated.");

            item.SystemType = typeof(TEntity).FullName;

            var result = await _client.ReplaceDocumentAsync(document.SelfLink, item);

            var response = new TEntity();
            response.Hydrate(result.Resource);
            return response;
        }

        #endregion

        #region private methods

        private async Task<Document> GetDocumentAsync(Guid id)
        {
            await Initialize();

            var link = $"dbs/{_databaseName}/colls/{_collectionName}/docs/{id}";

            var result = await _client.ReadDocumentAsync(link);

            return result.Resource;
        }

        #endregion
    }
}
