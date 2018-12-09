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
    public class CosmosEntityStoreDeleteOkTest
    {
        CosmosEntityStore<TestCosmosEntity> _cosmosEntityStore;
        TestCosmosEntity _result;

        [TestInitialize]
        public async Task Initialize()
        {
            var log = LogManager.GetLogger(typeof(CosmosEntityStoreAddOkTest));

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
        public async Task CosmosEntityStoreDelete_Ok()
        {
            var deletedEntity = await _cosmosEntityStore.DeleteAsync(_result);

            Assert.AreEqual(_result.Id, deletedEntity.Id);
            Assert.AreEqual(_result.SystemType, deletedEntity.SystemType);
            Assert.AreEqual(_result.StringField, deletedEntity.StringField);

            try
            {
                var notExpectedResult = await _cosmosEntityStore.GetAsync(_result.Id.Value);

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
