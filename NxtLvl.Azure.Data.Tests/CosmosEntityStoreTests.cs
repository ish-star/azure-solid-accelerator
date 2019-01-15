using log4net;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NxtLvl.Azure.Data.Tests
{
    public class CosmosEntityStoreTests
    {
        [Fact]
        public void CosmosEntityStore_DefaultConstructor_NullLog()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CosmosEntityStore<CosmosDocument>(null, null, null, null, null));

            Assert.Equal("Value cannot be null.\r\nParameter name: log", ex.Message);
        }

        [Fact]
        public void CosmosEntityStore_DefaultConstructor_NullUri()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new CosmosEntityStore<CosmosDocument>(log.Object, null, null, null, null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: uri", ex.Message);
        }

        [Fact]
        public void CosmosEntityStore_DefaultConstructor_NullAuthKey()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new CosmosEntityStore<CosmosDocument>(log.Object, "http://localhost/", null, null, null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: authKey", ex.Message);
        }

        [Fact]
        public void CosmosEntityStore_DefaultConstructor_NullDatabaseName()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new CosmosEntityStore<CosmosDocument>(log.Object, "http://localhost/", "authKey", null, null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: databaseName", ex.Message);
        }

        [Fact]
        public void CosmosEntityStore_DefaultConstructor_NullCollectionName()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new CosmosEntityStore<CosmosDocument>(log.Object, "http://localhost/", "authKey", "databaseName", null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: collectionName", ex.Message);
        }

        [Fact]
        public async Task CosmosEntityStore_AddAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var cosmosEntityStore = new CosmosEntityStore<CosmosDocument>(log.Object, "http://localhost/", "authKey", "databaseName", "collectionName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => cosmosEntityStore.AddAsync<CosmosDocument>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }

        [Fact]
        public async Task CosmosEntityStore_DeleteAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var cosmosEntityStore = new CosmosEntityStore<CosmosDocument>(log.Object, "http://localhost/", "authKey", "databaseName", "collectionName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => cosmosEntityStore.DeleteAsync<CosmosDocument>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }

        [Fact]
        public async Task CosmosEntityStore_FindAsync_NullPredicate()
        {
            var log = new Mock<ILog>();

            var cosmosEntityStore = new CosmosEntityStore<CosmosDocument>(log.Object, "http://localhost/", "authKey", "databaseName", "collectionName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => cosmosEntityStore.FindAsync<CosmosDocument>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: predicate", ex.Message);
        }

        [Fact]
        public async Task CosmosEntityStore_UpdateAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var cosmosEntityStore = new CosmosEntityStore<CosmosDocument>(log.Object, "http://localhost/", "authKey", "databaseName", "collectionName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => cosmosEntityStore.UpdateAsync<CosmosDocument>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }
    }
}
