using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreDeleteNotFoundTest
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

            _testEntity = new TestTableStorageEntity(new TableStorageId("TestPartition", Guid.NewGuid().ToString())) { StringField = "TestString" };
        }

        [TestMethod]
        public async Task TableStorageEntityStoreDelete_NotFound()
        {
            try
            {
                _testEntity.ETag = "*";
                var notExpectedResult = await _tableStorageEntityStore.DeleteAsync(_testEntity);

                Assert.Fail("A StorageException was expected to be thrown but no exception was thrown.");
            }
            catch (StorageException ex)
            {
                Assert.AreEqual("Not Found", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail($"A StorageException was expected to be thrown but an exception of type {ex.GetType().Name} was thrown.");
            }
        }
    }
}
