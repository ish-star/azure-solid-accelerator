using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    public interface ITableQuery<TItem>
        where TItem : ITableEntity, new()
    {
        Task<IList<TItem>> FindAsync(TableQuery<TItem> query);
    }
}
