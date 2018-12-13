using Xunit;

namespace NxtLvl.Azure.Data.Tests
{
    public class TableStorageIdTests
    {
        [Fact]
        public void TableStorageId_DefaultConstructor()
        {
            var tableStorageId = new TableStorageId();

            Assert.Null(tableStorageId.PartitionKey);
            Assert.Null(tableStorageId.RowKey);
        }

        [Fact]
        public void TableStorageId_PreferredConstructor()
        {
            var tableStorageId = new TableStorageId("TestPartitionKey", "TestRowKey");

            Assert.Equal("TestPartitionKey", tableStorageId.PartitionKey);
            Assert.Equal("TestRowKey", tableStorageId.RowKey);
        }

        [Fact]
        public void TableStorageId_Equals_SameTypeAndValues_Equal()
        {
            var tableStorageId1 = new TableStorageId("TestPartitionKey", "TestRowKey");
            var tableStorageId2 = new TableStorageId("TestPartitionKey", "TestRowKey");

            Assert.True(tableStorageId1.Equals(tableStorageId2));
        }

        [Fact]
        public void TableStorageId_Equals_SameTypeDifferentPartitionKey_NotEqual()
        {
            var tableStorageId1 = new TableStorageId("TestPartitionKey", "TestRowKey");
            var tableStorageId2 = new TableStorageId("TestPartition", "TestRowKey");

            Assert.False(tableStorageId1.Equals(tableStorageId2));
        }

        [Fact]
        public void TableStorageId_Equals_SameTypeDifferentRowKey_NotEqual()
        {
            var tableStorageId1 = new TableStorageId("TestPartitionKey", "TestRowKey");
            var tableStorageId2 = new TableStorageId("TestPartitionKey", "TestRow");

            Assert.False(tableStorageId1.Equals(tableStorageId2));
        }

        [Fact]
        public void TableStorageId_Equals_WrongType_NotEqual()
        {
            var tableStorageId1 = new TableStorageId("TestPartitionKey", "TestRowKey");

            Assert.False(tableStorageId1.Equals(new object()));
        }

        [Fact]
        public void TableStorageId_GetHashCode_SameValues_HashCodesEqual()
        {
            var tableStorageId1 = new TableStorageId("TestPartitionKey", "TestRowKey");
            var tableStorageId2 = new TableStorageId("TestPartitionKey", "TestRowKey");

            Assert.Equal(tableStorageId1.GetHashCode(), tableStorageId2.GetHashCode());
        }

        [Fact]
        public void TableStorageId_GetHashCode_DifferentPartitionKeys_HashCodesNotEqual()
        {
            var tableStorageId1 = new TableStorageId("TestPartitionKey", "TestRowKey");
            var tableStorageId2 = new TableStorageId("TestPartition", "TestRowKey");

            Assert.NotEqual(tableStorageId1.GetHashCode(), tableStorageId2.GetHashCode());
        }

        [Fact]
        public void TableStorageId_GetHashCode_DifferentRowKeys_HashCodesNotEqual()
        {
            var tableStorageId1 = new TableStorageId("TestPartitionKey", "TestRowKey");
            var tableStorageId2 = new TableStorageId("TestPartitionKey", "TestRow");

            Assert.NotEqual(tableStorageId1.GetHashCode(), tableStorageId2.GetHashCode());
        }

        [Fact]
        public void TableStorageId_GetHashCode_NullValues_HashCodesEqual()
        {
            var tableStorageId1 = new TableStorageId();
            var tableStorageId2 = new TableStorageId();

            Assert.Equal(tableStorageId1.GetHashCode(), tableStorageId2.GetHashCode());
        }
    }
}
