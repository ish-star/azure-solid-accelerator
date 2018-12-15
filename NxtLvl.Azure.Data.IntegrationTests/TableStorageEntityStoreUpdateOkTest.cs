using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreUpdateOkTest
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
        public async Task TableStorageEntityStoreUpdate_Ok()
        {
            _result.StringField = "NewStringValue";
            _result = await _tableStorageEntityStore.UpdateAsync(_result);

            var gottenAfterUpdate = await _tableStorageEntityStore.GetAsync<TestTableStorageEntity>(_result.Id);

            Assert.AreEqual(_result.Id, gottenAfterUpdate.Id);
            Assert.AreEqual(_result.StringField, gottenAfterUpdate.StringField);
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
