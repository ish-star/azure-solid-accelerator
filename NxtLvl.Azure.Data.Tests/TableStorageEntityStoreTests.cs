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
            var ex = Assert.Throws<ArgumentNullException>(() => new TableStorageEntityStore<TableEntity>(null, null, null));

            Assert.Equal("Value cannot be null.\r\nParameter name: log", ex.Message);
        }

        [Fact]
        public void TableStorageEntityStore_DefaultConstructor_NullConnection()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new TableStorageEntityStore<TableEntity>(log.Object, null, null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: connection", ex.Message);
        }

        [Fact]
        public void TableStorageEntityStore_DefaultConstructor_NullTableName()
        {
            var log = new Mock<ILog>();

            var ex = Assert.Throws<ArgumentException>(() => new TableStorageEntityStore<TableEntity>(log.Object, "TestConnection", null));

            Assert.Equal("Value cannot be null or whitespace.\r\nParameter name: tableName", ex.Message);
        }

        [Fact]
        public async Task TableStorageEntityStore_AddAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var tableStorageEntityStore = new TableStorageEntityStore<TableEntity>(log.Object, "TestConnection", "TableName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => tableStorageEntityStore.AddAsync(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }

        [Fact]
        public async Task TableStorageEntityStore_DeleteAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var tableStorageEntityStore = new TableStorageEntityStore<TableEntity>(log.Object, "TestConnection", "TableName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => tableStorageEntityStore.DeleteAsync(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }

        [Fact]
        public async Task TableStorageEntityStore_UpdateAsync_NullItem()
        {
            var log = new Mock<ILog>();

            var tableStorageEntityStore = new TableStorageEntityStore<TableEntity>(log.Object, "TestConnection", "TableName");

            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => tableStorageEntityStore.UpdateAsync(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: item", ex.Message);
        }
    }
}
