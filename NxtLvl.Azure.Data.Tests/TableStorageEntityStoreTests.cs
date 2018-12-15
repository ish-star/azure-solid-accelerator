using log4net;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NxtLvl.Azure.Data.Tests
{
    public class TableStorageEntityStoreTests
    {
        [Fact]
        public void TableStorageEntityStore_DefaultConstructor_NullLog()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new TableStorageEntityStore<TableStorageEntity>(null, null, null));

            Assert.Equal("Value cannot be null.\r\nParameter name: log", ex.Message);
        }

        [Fact]
        public void TableStorageEntityStore_DefaultConstructor_NullConnection()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new TableStorageEntityStore<TableStorageEntity>(log.Object, null, null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: connection", ex.Message);
        }

        [Fact]
        public void TableStorageEntityStore_DefaultConstructor_NullTableName()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new TableStorageEntityStore<TableStorageEntity>(log.Object, "TestConnection", null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: tableName", ex.Message);
        }

        [Fact]
        public async Task TableStorageEntityStore_AddAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var tableStorageEntityStore = new TableStorageEntityStore<TableStorageEntity>(log.Object, "TestConnection", "TableName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => tableStorageEntityStore.AddAsync<TableStorageEntity>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }

        [Fact]
        public async Task TableStorageEntityStore_DeleteAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var tableStorageEntityStore = new TableStorageEntityStore<TableStorageEntity>(log.Object, "TestConnection", "TableName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => tableStorageEntityStore.DeleteAsync<TableStorageEntity>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }

        [Fact]
        public async Task TableStorageEntityStore_UpdateAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var tableStorageEntityStore = new TableStorageEntityStore<TableStorageEntity>(log.Object, "TestConnection", "TableName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => tableStorageEntityStore.UpdateAsync<TableStorageEntity>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }

        [Fact]
        public async Task TableStorageEntityStore_FindAsync_NullQuery()
        {
            var log = new Mock<ILog>();

            var tableStorageEntityStore = new TableStorageEntityStore<TableStorageEntity>(log.Object, "TestConnection", "TableName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => tableStorageEntityStore.FindAsync<TableStorageEntity>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: query", ex.Message);
        }
    }
}
