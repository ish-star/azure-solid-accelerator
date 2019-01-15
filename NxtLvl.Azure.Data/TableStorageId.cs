namespace NxtLvl.Azure.Data
{
    /// <summary>
    /// A structure representing the primary key for all Azure TableStorage records.
    /// </summary>
    public struct TableStorageId
    {
        #region fields

        public const string ParitionKeyField = "PartitionKey";
        public const string RowKeyField = "RowKey";

        #endregion

        #region properties

        /// <summary>
        /// Tables are partitioned to support load balancing across storage nodes. A table's entities are organized by partition. A partition is a consecutive range of entities possessing the same partition key value. The partition key is a unique identifier for the partition within a given table, specified by the PartitionKey property. The partition key forms the first part of an entity's primary key. The partition key may be a string value up to 1 KB in size.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model#partitionkey-property"/>
        /// <remarks>
        /// You must include the PartitionKey property in every insert, update, and delete operation.
        /// </remarks>
        public string PartitionKey { get; set; }

        /// <summary>
        /// The second part of the primary key is the row key, specified by the RowKey property. The row key is a unique identifier for an entity within a given partition. Together the PartitionKey and RowKey uniquely identify every entity within a table.  The row key is a string value that may be up to 1 KB in size.
        /// </summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model#rowkey-property"/>
        /// <remarks>
        /// You must include the RowKey property in every insert, update, and delete operation.
        /// </remarks>
        public string RowKey { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// A structure representing the primary key for all Azure TableStorage records.
        /// </summary>
        /// <param name="partitionKey">The first part of the primary key of the TableStorage record.</param>
        /// <param name="rowKey">The second part of the primary key of the TableStorage record.</param>
        public TableStorageId(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
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

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            var partiontionHash = PartitionKey == null ? 3 : PartitionKey.GetHashCode();
            var rowHash = RowKey == null ? 7 : RowKey.GetHashCode();

            return partiontionHash * rowHash;
        }

        #endregion
    }
}
