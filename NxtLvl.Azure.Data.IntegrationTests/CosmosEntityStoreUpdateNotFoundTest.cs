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
    public class CosmosEntityStoreUpdateNotFoundTest
    {
        CosmosEntityStore<TestCosmosEntity> _cosmosEntityStore;
        TestCosmosEntity _testEntity;

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

            _testEntity = new TestCosmosEntity() { Id = Guid.NewGuid(), StringField = "TestString" };
        }

        [TestMethod]
        public async Task CosmosEntityStoreUpdate_NotFound()
        {
            try
            {
                var notExpectedResult = await _cosmosEntityStore.UpdateAsync(_testEntity);

                Assert.Fail("A DocumentClientException was expected to be thrown but no exception was thrown.");
            }
            catch (DocumentClientException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.Fail($"A DocumentClientException was expected to be thrown but an exception of type {ex.GetType().Name} was thrown.");
            }
        }
    }
}
