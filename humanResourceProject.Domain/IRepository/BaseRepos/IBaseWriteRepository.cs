using humanResourceProject.Domain.Entities.Abstract;

namespace humanResourceProject.Domain.IRepository.BaseRepos
{
    public interface IBaseWriteRepository<T> where T : class, IBaseEntity, new()
    {
        Task<bool> Insert(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(Guid id);
        Task<int> SaveChangesAsync();
        void DetachEntity(T entity);
    }
}
