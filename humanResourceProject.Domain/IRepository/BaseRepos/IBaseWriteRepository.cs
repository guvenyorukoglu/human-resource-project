using humanResourceProject.Domain.Entities.Abstract;

namespace humanResourceProject.Domain.IRepository.BaseRepos
{
    public interface IBaseWriteRepository<T> where T : class, IBaseEntity, new()
    {
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(Guid id);
        Task<int> SaveChangesAsync();

    }
}
