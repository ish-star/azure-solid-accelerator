using System.Threading.Tasks;

namespace NxtLvl.Azure.Data
{
    public interface IEntityStore<TEntity, TId>
    {
        Task<TEntity> CreateAsync(TEntity item);
        Task<TEntity> DeleteAsync(TEntity item);
        Task<TEntity> UpdateAsync(TEntity item);
        Task<TEntity> GetAsync(TId id);
    }
}
