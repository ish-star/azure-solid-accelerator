using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NxtLvl.Core.Common;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    public class TableStorageEntityStore<TEntity> : IEntityStore<TEntity, TableStorageId>
        where TEntity : class, ITableEntity, new()
    {
        readonly string _connection, _tableName;
        readonly ILog _log;

        CloudTable _cloudTable;
        bool _initialized;

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

        public async Task<TEntity> AddAsync(TEntity item)
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            var operation = TableOperation.Insert(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        public async Task<TEntity> DeleteAsync(TEntity item)
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            var operation = TableOperation.Delete(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        public async Task<TEntity> GetAsync(TableStorageId id)
        {
            await Initialize();

            var operation = TableOperation.Retrieve<TEntity>(id.PartitionKey, id.RowKey);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        public async Task<TEntity> UpdateAsync(TEntity item)
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            await Initialize();

            var operation = TableOperation.Replace(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        //private void ProcessStatusCode(int statusCode)
        //{
        //    if (statusCode > )
        //}
    }
}
