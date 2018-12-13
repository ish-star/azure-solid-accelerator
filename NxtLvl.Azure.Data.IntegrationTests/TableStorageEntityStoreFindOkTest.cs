using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreFindOkTest
    {
        TableStorageEntityStore<TestTableStorageEntity> _tableStorageEntityStore;
        const string ParitionKeyToFind = "PartitionKeyToFind";
        const string ParitionKeyToNotFind = "PartitionKeyToNotFind";
        const int NumberToFind = 22;
        const int NumberToNotFind = 33;
        List<TestTableStorageEntity> _testEntities;

        [TestInitialize]
        public async Task Initialize()
        {
            var log = LogManager.GetLogger(GetType());

            _tableStorageEntityStore = new TableStorageEntityStore<TestTableStorageEntity>(log,
                                                                                           Configuration.GetTableStorageConfig()["connection"],
                                                                                           Configuration.GetTableStorageConfig()["tableName"]);
            await _tableStorageEntityStore.Initialize();

            _testEntities = new List<TestTableStorageEntity>(NumberToFind + NumberToNotFind);

            for (int i = 0; i < NumberToFind; i++)
            {
                var testEntity = new TestTableStorageEntity(new TableStorageId(ParitionKeyToFind, Guid.NewGuid().ToString()));

                var result = await _tableStorageEntityStore.AddAsync(testEntity);

                _testEntities.Add(result);
            }

            for (int i = 0; i < NumberToNotFind; i++)
            {
                var testEntity = new TestTableStorageEntity(new TableStorageId(ParitionKeyToNotFind, Guid.NewGuid().ToString()));

                var result = await _tableStorageEntityStore.AddAsync(testEntity);

                _testEntities.Add(result);
            }
        }

        [TestMethod]
        public async Task TableStorageEntityStoreFind_Ok()
        {
            var query = new TableQuery<TestTableStorageEntity>().Where(TableQuery.GenerateFilterCondition(TableStorageId.ParitionKeyField, QueryComparisons.Equal, ParitionKeyToFind));

            var found = await _tableStorageEntityStore.FindAsync(query);

            Assert.AreEqual(NumberToFind, found.Count);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_testEntities != null)
            {
                foreach (var item in _testEntities)
                {
                    var deleted = await _tableStorageEntityStore.DeleteAsync(item);
                }
            }
        }
    }
}
