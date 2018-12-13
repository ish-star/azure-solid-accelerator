using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreDeleteOkTest
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
        public async Task TableStorageEntityStoreDelete_Ok()
        {
            var deleted = await _tableStorageEntityStore.DeleteAsync(_result);

            Assert.AreEqual(_result.Id, deleted.Id);
            Assert.AreEqual(_result.StringField, deleted.StringField);

            var notExpectedResult = await _tableStorageEntityStore.GetAsync(_result.Id);

            Assert.IsNull(notExpectedResult);
        }
    }
}
