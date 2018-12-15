using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    public interface ILinqQuery
    {
        Task<IList<TItem>> FindAsync<TItem>(Expression<Func<TItem, bool>> predicate);
    }
}
