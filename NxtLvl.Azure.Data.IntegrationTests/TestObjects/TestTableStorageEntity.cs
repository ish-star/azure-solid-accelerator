using Microsoft.WindowsAzure.Storage.Table;

namespace NxtLvl.Azure.Data.IntegrationTests.TestObjects
{
    public class TestTableStorageEntity : TableStorageEntity
    {
        public string StringField { get; set; }

        public TestTableStorageEntity()
        { }

        public TestTableStorageEntity(TableStorageId id)
        {
            PartitionKey = id.PartitionKey;
            RowKey = id.RowKey;
        }
    }
}
