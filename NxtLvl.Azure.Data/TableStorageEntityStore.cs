﻿using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NxtLvl.Core.Common;
using System;
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

            _log.Info($"The TableStorageEntityStore for Table:{tableName} and has begun construction.");

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
        }

        public async Task<TEntity> CreateAsync(TEntity item)
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            var operation = TableOperation.Insert(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        public async Task<TEntity> DeleteAsync(TEntity item)
        {
            Validate.ArgumentIsNotNull(item, nameof(item));

            var operation = TableOperation.Delete(item);

            var result = await _cloudTable.ExecuteAsync(operation);

            return (TEntity)result.Result;
        }

        public Task<TEntity> GetAsync(TableStorageId id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }
    }
}