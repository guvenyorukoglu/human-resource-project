using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.IRepository;
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

        public async Task Insert(T entity)
        {
            await table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            table.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            //Delete operation will be made by changing entity's status in service layer.
            await _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
