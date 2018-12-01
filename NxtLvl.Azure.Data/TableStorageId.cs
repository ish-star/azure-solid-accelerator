namespace NxtLvl.Azure.Data
{
    public class TableStorageId
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public TableStorageId()
        { }

        public TableStorageId(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }
    }
}
