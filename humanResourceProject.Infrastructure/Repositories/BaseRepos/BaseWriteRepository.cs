using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.IRepository.BaseRepos;
using humanResourceProject.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace humanResourceProject.Infrastructure.Repositories.BaseRepos
{
    public class BaseWriteRepository<T> : IBaseWriteRepository<T> where T : class, IBaseEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> table;

        public BaseWriteRepository(AppDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public async Task<bool> Insert(T entity)
        {
            await table.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(T entity)
        {
            table.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            //Delete operation will be made by changing entity's status in service layer.
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void DetachEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
