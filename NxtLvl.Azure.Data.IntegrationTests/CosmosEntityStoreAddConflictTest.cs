using log4net;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class CosmosEntityStoreAddConflictTest
    {
        CosmosEntityStore<TestCosmosEntity> _cosmosEntityStore;
        TestCosmosEntity _result;

        [TestInitialize]
        public async Task Initialize()
        {
            var log = LogManager.GetLogger(GetType());

            _cosmosEntityStore = new CosmosEntityStore<TestCosmosEntity>(log,
                                                                         Configuration.GetComsosConfig()["uri"],
                                                                         Configuration.GetComsosConfig()["authKey"],
                                                                         Configuration.GetComsosConfig()["databaseName"],
                                                                         Configuration.GetComsosConfig()["collectionName"]);
            await _cosmosEntityStore.Initialize();

            var testEntity = new TestCosmosEntity() { StringField = "TestString" };

            _result = await _cosmosEntityStore.AddAsync(testEntity);
        }

        [TestMethod]
        public async Task CosmosEntityStoreAdd_Conflict()
        {
            try
            {
                var notExpectedResult = await _cosmosEntityStore.AddAsync(_result);

                Assert.Fail("A DocumentClientException was expected to be thrown but no exception was thrown.");
            }
            catch (DocumentClientException ex)
            {
                Assert.AreEqual(HttpStatusCode.Conflict, ex.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.Fail($"A DocumentClientException was expected to be thrown but an exception of type {ex.GetType().Name} was thrown.");
            }
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_result != null)
            {
                var deleted = await _cosmosEntityStore.DeleteAsync(_result);
            }
        }
    }
}
