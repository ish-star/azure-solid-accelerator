using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreAddOkTest
    {
        TableStorageEntityStore<TableStorageEntity> _tableStorageEntityStore;
        TestTableStorageEntity _result;

        [TestInitialize]
        public async Task Initialize()
        {
            var log = LogManager.GetLogger(GetType());

            _tableStorageEntityStore = new TableStorageEntityStore<TableStorageEntity>(log,
                                                                                           Configuration.GetTableStorageConfig()["connection"],
                                                                                           Configuration.GetTableStorageConfig()["tableName"]);
            await _tableStorageEntityStore.Initialize();
        }

        [TestMethod]
        public async Task TableStorageEntityStoreAdd_Ok()
        {
            var testEntity = new TestTableStorageEntity(new TableStorageId("TestPartition", Guid.NewGuid().ToString())) { StringField = "TestString" };

            _result = await _tableStorageEntityStore.AddAsync(testEntity);

            Assert.AreEqual(testEntity.PartitionKey, _result.PartitionKey);
            Assert.AreEqual(testEntity.RowKey, _result.RowKey);
            Assert.AreEqual(testEntity.StringField, _result.StringField);

            var gotten = await _tableStorageEntityStore.GetAsync<TestTableStorageEntity>(_result.Id);

            Assert.AreEqual(testEntity.PartitionKey, gotten.PartitionKey);
            Assert.AreEqual(testEntity.RowKey, gotten.RowKey);
            Assert.AreEqual(testEntity.StringField, gotten.StringField);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_result != null)
            {
                var deleted = await _tableStorageEntityStore.DeleteAsync(_result);
            }
        }
    }
}
