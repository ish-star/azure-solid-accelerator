using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NxtLvl.Core.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    /// <summary>
    /// Class that encapsluates the complexity of doing simple data interactions with the Azure TableStorage platform.
    /// </summary>
    /// <typeparam name="TBase">The base type the CosmosEntityStore is configured to work with.</typeparam>
    public class TableStorageEntityStore<TBase> : IEntityStore<TBase, TableStorageId>, ITableQuery
        where TBase : TableStorageEntity
    {
        #region fields

        readonly string _connection, _tableName;
        readonly ILog _log;

        CloudTable _cloudTable;
        bool _initialized;

        #endregion

        #region constructor

        /// <summary>
        /// The constructor that specifies the dependencies and metadata concerning the instance of the Azure TableStorage platform to which you intend to interact.
        /// </summary>
        /// <param name="log">The log4net's ILog interface which will be used to log class initialization and other events.</param>
        /// <param name="connection">The connection string for the TableStorage instance to which you want to interact.</param>
        /// <param name="tableName">The name of the Table within the TableStorage instance to which you want to interact.</param>
        public TableStorageEntityStore(ILog log, string connection, string tableName)
        {
            Validate.ArgumentIsNotNull(log, nameof(log));
            Validate.ArgumentIsNotNullOrEmpty(connection, nameof(connection));
            Validate.ArgumentIsNotNullOrEmpty(tableName, nameof(tableName));

            log.Info($"The TableStorageEntityStore for Table:{tableName} and has begun construction.");

            _log = log;
            _connection = connection;
            _tableName = tableName;

            _log.Info($"The TableStorageEntityStore for Table:{tableName} and has finishded construction.");
        }

        #endregion

        #region public methods

        /// <summary>
        /// Uses the information provided in the constructor to access your TableStorage instance and ensures a connection can be made.  This method also ensures that the database and collection exist and are ready to accept data interactions.
        /// </summary>
        /// <remarks>
        /// This method should be called only once per instance.  It is recommended to call this method during the initialization or dependency configuration stage of your application to warm up the underlying components to avoid cold call latency.
        /// </remarks>
        public async Task Initialize()
        {
            if (_initialized)
                return;

            var account = CloudStorageAccount.Parse(_connection);
            var client = account.CreateCloudTableClient();

            _cloudTable = client.GetTableReference(_tableName);

            await _cloudTable.CreateIfNotExistsAsync();

            _initialized = true;
        }

        /// <summary>
        /// Add the provided Entity to the TableStorage table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to add.</param>
        /// <returns>The added Entity.</returns>
        public async Task<TEntity> AddAsync<TEntity>(TEntity item)
            where TEntity : TBase, new()
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            var operation = TableOperation.Insert(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        /// <summary>
        /// Delete the provided Entity from the TableStorage table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to delete.</param>
        /// <returns>The deleted Entity.</returns>
        public async Task<TEntity> DeleteAsync<TEntity>(TEntity item)
            where TEntity : TBase, new()
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            var operation = TableOperation.Delete(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        /// <summary>
        /// Get the Entity associated with the provided Id from the TableStorage table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="id">The Id value of the Entity to get.</param>
        /// <returns>The Entity associated with the provided Id value.</returns>
        public async Task<TEntity> GetAsync<TEntity>(TableStorageId id)
            where TEntity : TBase, new()
        {
            await Initialize();

            var operation = TableOperation.Retrieve<TEntity>(id.PartitionKey, id.RowKey);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        /// <summary>
        /// Add the provided Entity to the CosmosDB table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to update.</param>
        /// <returns>The updated Entity.</returns>
        public async Task<TEntity> UpdateAsync<TEntity>(TEntity item)
            where TEntity : TBase, new()
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            var operation = TableOperation.Replace(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        /// <summary>
        /// Searches for records that meet the criteria specified in the predicate.
        /// </summary>
        /// <typeparam name="TItem">The type to be used in this method invocation.</typeparam>
        /// <param name="query">The criteria to be used in the query operation.</param>
        /// <returns>Any records that meet the search criteria cast into the type specified by the TItem type parameter.</returns>
        public async Task<IList<TItem>> FindAsync<TItem>(TableQuery<TItem> query)
            where TItem : TableStorageEntity, new()
        {
            Validate.ArgumentIsNotNull(query, nameof(query));

            await Initialize();

            var results = new List<TItem>();
            TableContinuationToken token = null;

            do
            {
                var segment = await _cloudTable.ExecuteQuerySegmentedAsync(query, token);
                token = segment.ContinuationToken;
                results.AddRange(segment.Results);

            } while (token != null);

            return results;
        }

        #endregion
    }
}
