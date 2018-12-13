namespace NxtLvl.Azure.Data
{
    public struct TableStorageId
    {
        public const string ParitionKeyField = "PartitionKey";
        public const string RowKeyField = "RowKey";

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public TableStorageId(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TableStorageId))
            {
                return false;
            }

            var id = (TableStorageId)obj;

            if (id.PartitionKey != PartitionKey)
            {
                return false;
            }

            if (id.RowKey != RowKey)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var partiontionHash = PartitionKey == null ? 3 : PartitionKey.GetHashCode();
            var rowHash = RowKey == null ? 7 : RowKey.GetHashCode();

            return partiontionHash * rowHash;
        }
    }
}
