using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class CosmosEntityStoreFindOkTest
    {
        CosmosEntityStore<CosmosDocument> _cosmosEntityStore;
        TestCosmosEntity _result1, _result2, _result3;

        [TestInitialize]
        public async Task Initialize()
        {
            var log = LogManager.GetLogger(GetType());

            _cosmosEntityStore = new CosmosEntityStore<CosmosDocument>(log,
                                                                        Configuration.GetComsosConfig()["uri"],
                                                                        Configuration.GetComsosConfig()["authKey"],
                                                                        Configuration.GetComsosConfig()["databaseName"],
                                                                        Configuration.GetComsosConfig()["collectionName"]);
            await _cosmosEntityStore.Initialize();

            var testEntity = new TestCosmosEntity() { StringField = "TestString" };

            _result1 = await _cosmosEntityStore.AddAsync(testEntity);

            var testEntity2 = new TestCosmosEntity() { StringField = "TestString" };

            _result2 = await _cosmosEntityStore.AddAsync(testEntity2);

            var testEntity3 = new TestCosmosEntity() { StringField = "DoNotFindTestString" };

            _result3 = await _cosmosEntityStore.AddAsync(testEntity3);
        }

        [TestMethod]
        public async Task CosmosEntityStoreFind_Ok()
        {
            var foundEntities = await _cosmosEntityStore.FindAsync<TestCosmosEntity>(x => x.StringField == "TestString");

            Assert.AreEqual(2, foundEntities.Count);
            Assert.AreEqual(_result1.Id, foundEntities[0].Id);
            Assert.AreEqual(_result1.SystemType, foundEntities[0].SystemType);
            Assert.AreEqual(_result1.StringField, foundEntities[0].StringField);
            Assert.AreEqual(_result2.Id, foundEntities[1].Id);
            Assert.AreEqual(_result2.SystemType, foundEntities[1].SystemType);
            Assert.AreEqual(_result2.StringField, foundEntities[1].StringField);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            if (_result1 != null)
            {
                var deleted = await _cosmosEntityStore.DeleteAsync(_result1);
            }

            if (_result2 != null)
            {
                var deleted = await _cosmosEntityStore.DeleteAsync(_result2);
            }

            if (_result3 != null)
            {
                var deleted = await _cosmosEntityStore.DeleteAsync(_result3);
            }
        }
    }
}
