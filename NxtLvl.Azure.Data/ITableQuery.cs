using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    public interface ITableQuery
    {
        /// <summary>
        /// Searches for records that meet the criteria specified in the predicate.
        /// </summary>
        /// <typeparam name="TItem">The type to be used in this method invocation.</typeparam>
        /// <param name="query">The criteria to be used in the query operation.</param>
        /// <returns>Any records that meet the search criteria cast into the type specified by the TItem type parameter.</returns>
        Task<IList<TItem>> FindAsync<TItem>(TableQuery<TItem> query)
            where TItem : TableStorageEntity, new();
    }
}
