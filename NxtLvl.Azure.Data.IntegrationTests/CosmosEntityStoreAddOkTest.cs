using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NxtLvl.Azure.Data.IntegrationTests.TestObjects;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data.IntegrationTests
{
    [TestClass]
    public class CosmosEntityStoreAddOkTest
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
        }

        [TestMethod]
        public async Task CosmosEntityStoreAdd_Ok()
        {
            var testEntity = new TestCosmosEntity() { StringField = "TestString" };

            _result = await _cosmosEntityStore.AddAsync(testEntity);

            Assert.IsNotNull(_result.Id);
            Assert.IsNotNull(_result.SystemType);
            Assert.AreEqual(testEntity.StringField, _result.StringField);

            var gotten = await _cosmosEntityStore.GetAsync(_result.Id.Value);

            Assert.AreEqual(_result.Id, gotten.Id);
            Assert.AreEqual(_result.SystemType, gotten.SystemType);
            Assert.AreEqual(_result.StringField, gotten.StringField);
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
