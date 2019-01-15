using Microsoft.WindowsAzure.Storage.Table;

namespace NxtLvl.Azure.Data
{
    /// <summary>
    /// Base class that can be used when defining Entities to use with the Azure Table Storage platform.
    /// </summary>
    public class TableStorageEntity : TableEntity
    {
        #region properties

        /// <summary>
        /// The identifier of the table storage record.
        /// </summary>
        public TableStorageId Id { get { return new TableStorageId(PartitionKey, RowKey); } }

        #endregion

        #region constructors

        /// <summary>
        /// Base class that can be used when defining Entities to use with the Azure Table Storage platform.
        /// </summary>
        public TableStorageEntity()
        { }

        /// <summary>
        /// Base class that can be used when defining Entities to use with the Azure Table Storage platform.
        /// </summary>
        /// <param name="id">The identifier of the table storage record.</param>
        public TableStorageEntity(TableStorageId id)
        {
            PartitionKey = id.PartitionKey;
            RowKey = id.RowKey;
        }

        #endregion
    }
}
