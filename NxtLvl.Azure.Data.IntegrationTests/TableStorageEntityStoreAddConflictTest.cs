using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class TableStorageEntityStoreAddConflictTest
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
        public async Task TableStorageEntityStoreAdd_Conflict()
        {
            try
            {
                var notExpectedResult = await _tableStorageEntityStore.AddAsync(_result);

                Assert.Fail("A StorageException was expected to be thrown but no exception was thrown.");
            }
            catch (StorageException ex)
            {
                Assert.AreEqual(HttpStatusCode.Conflict.ToString(), ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail($"A StorageException was expected to be thrown but an exception of type {ex.GetType().Name} was thrown.");
            }
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
