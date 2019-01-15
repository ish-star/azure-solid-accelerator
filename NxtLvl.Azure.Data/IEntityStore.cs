using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    /// <summary>
    /// Interface for a generic storage of application Entities.
    /// </summary>
    /// <typeparam name="TBase">The base type the implementation is configured to work with.</typeparam>
    /// <typeparam name="TId">The type of the Id used by the implementation of this interface.</typeparam>
    public interface IEntityStore<TBase, TId>
        where TBase : class
    {
        /// <summary>
        /// Add the provided Entity to the Entity Store.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to add.</param>
        /// <returns>The added Entity.</returns>
        Task<TEntity> AddAsync<TEntity>(TEntity item)
            where TEntity : TBase, new();

        /// <summary>
        /// Delete the provided Entity from the Entity Store.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to delete.</param>
        /// <returns>The deleted Entity.</returns>
        Task<TEntity> DeleteAsync<TEntity>(TEntity item)
            where TEntity : TBase, new();

        /// <summary>
        /// Get the Entity associated with the provided Id from the Entity Store.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="id">The Id value of the Entity to get.</param>
        /// <returns>The Entity associated with the provided Id value.</returns>
        Task<TEntity> GetAsync<TEntity>(TId id)
            where TEntity : TBase, new();

        /// <summary>
        /// Add the provided Entity to the Entity Store.
        /// </summary>
        /// <typeparam name="TEntity">The type to be used in this method invocation.</typeparam>
        /// <param name="item">The Entity to add.</param>
        /// <returns>The added Entity.</returns>
        Task<TEntity> UpdateAsync<TEntity>(TEntity item)
            where TEntity : TBase, new();
    }
}
