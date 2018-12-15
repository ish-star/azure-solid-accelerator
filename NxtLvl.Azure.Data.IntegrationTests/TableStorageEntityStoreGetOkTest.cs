using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreGetOkTest
    {
        TableStorageEntityStore<TestTableStorageEntity> _tableStorageEntityStore;
        TestTableStorageEntity _result;

        [TestInitialize]
        public async Task Initialize()
        {
            var log = LogManager.GetLogger(GetType());

            _tableStorageEntityStore = new TableStorageEntityStore<TestTableStorageEntity>(log,
                                                                                           Configuration.GetTableStorageConfig()["connection"],
                                                                                           Configuration.GetTableStorageConfig()["tableName"]);
            await _tableStorageEntityStore.Initialize();

            var testEntity = new TestTableStorageEntity(new TableStorageId("TestPartition", Guid.NewGuid().ToString())) { StringField = "TestString" };

            _result = await _tableStorageEntityStore.AddAsync(testEntity);
        }

        [TestMethod]
        public async Task TableStorageEntityStoreGet_Ok()
        {
            var gotten = await _tableStorageEntityStore.GetAsync<TestTableStorageEntity>(_result.Id);

            Assert.AreEqual(_result.PartitionKey, gotten.PartitionKey);
            Assert.AreEqual(_result.RowKey, gotten.RowKey);
            Assert.AreEqual(_result.StringField, gotten.StringField);
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
