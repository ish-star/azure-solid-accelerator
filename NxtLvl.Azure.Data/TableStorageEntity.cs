using Microsoft.WindowsAzure.Storage.Table;

namespace NxtLvl.Azure.Data
{
    public class TableStorageEntity : TableEntity
    {
        public TableStorageId Id { get { return new TableStorageId(PartitionKey, RowKey); } }

        public TableStorageEntity()
        { }

        public TableStorageEntity(TableStorageId id)
        {
            PartitionKey = id.PartitionKey;
            RowKey = id.RowKey;
        }
    }
}
