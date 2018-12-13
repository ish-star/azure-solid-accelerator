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
    public class CosmosEntityStore<TEntity> : IEntityStore<TEntity, Guid>, ILinqQuery<TEntity>
        where TEntity : CosmosDocument, new()
    {
        readonly string _authKey, _databaseName, _collectionName;
        readonly Uri _uri;
        readonly ILog _log;

        DocumentClient _client;
        Database _database;
        DocumentCollection _collection;
        bool _initialized;

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

        public async Task<TEntity> AddAsync(TEntity item)
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

        public async Task<TEntity> DeleteAsync(TEntity item)
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

        public async Task<TEntity> GetAsync(Guid id)
        {
            await Initialize();

            var document = await GetDocumentAsync(id);

            if (document == null)
                return null;

            var response = new TEntity();
            response.Hydrate(document);
            return response;
        }

        public async Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            Validate.ArgumentIsNotNull(predicate, nameof(predicate));

            await Initialize();

            return await Task.Run(() => _client.CreateDocumentQuery<TEntity>(_collection.DocumentsLink).Where(predicate).ToList());
        }

        public async Task<TEntity> UpdateAsync(TEntity item)
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

        private async Task<Document> GetDocumentAsync(Guid id)
        {
            await Initialize();

            var link = $"dbs/{_databaseName}/colls/{_collectionName}/docs/{id}";

            var result = await _client.ReadDocumentAsync(link);

            return result.Resource;
        }
    }
}
