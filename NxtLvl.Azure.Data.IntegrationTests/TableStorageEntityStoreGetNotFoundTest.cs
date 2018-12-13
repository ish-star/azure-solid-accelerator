using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Threading.Tasks;
namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreGetNotFoundTest
    {
        TableStorageEntityStore<TestTableStorageEntity> _tableStorageEntityStore;
        TestTableStorageEntity _testEntity;

        [TestInitialize]
        public async Task Initialize()
        {
            var log = LogManager.GetLogger(GetType());

            _tableStorageEntityStore = new TableStorageEntityStore<TestTableStorageEntity>(log,
                                                                                           Configuration.GetTableStorageConfig()["connection"],
                                                                                           Configuration.GetTableStorageConfig()["tableName"]);
            await _tableStorageEntityStore.Initialize();

            _testEntity = new TestTableStorageEntity(new TableStorageId("TestPartition", Guid.NewGuid().ToString())) { };
        }

        [TestMethod]
        public async Task TableStorageEntityStoreGet_NotFound()
        {
            var notExpectedResult = await _tableStorageEntityStore.GetAsync(_testEntity.Id);

            Assert.IsNull(notExpectedResult);
        }
    }
}
