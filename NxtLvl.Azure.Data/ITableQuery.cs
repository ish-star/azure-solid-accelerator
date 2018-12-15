using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    public interface ITableQuery
    {
        Task<IList<TItem>> FindAsync<TItem>(TableQuery<TItem> query)
            where TItem : TableStorageEntity, new();
    }
}
