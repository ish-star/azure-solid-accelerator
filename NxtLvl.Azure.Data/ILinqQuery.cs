using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    public interface ILinqQuery
    {
        /// <summary>
        /// Find any Entities that satisfy the conditions provided in the Linq predicate from the CosmosDB table.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="predicate">The condition the Entities must satifsy.</param>
        /// <returns>The Entities that satisfy the condition specified.</returns>
        Task<IList<TItem>> FindAsync<TItem>(Expression<Func<TItem, bool>> predicate);
    }
}
