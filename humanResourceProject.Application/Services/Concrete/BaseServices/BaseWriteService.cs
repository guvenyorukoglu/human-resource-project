using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.IRepository.BaseRepos;

namespace humanResourceProject.Application.Services.Concrete.BaseServices
{
    public class BaseWriteService<T> : IBaseWriteService<T> where T : class, IBaseEntity, new()
    {
        private readonly IBaseReadRepository<T> _readRepository;
        private readonly IBaseWriteRepository<T> _writeRepository;


        public BaseWriteService(IBaseWriteRepository<T> writeRepository, IBaseReadRepository<T> readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;

        }

        public async Task<bool> Delete(Guid id)
        {
            T entity = await _readRepository.GetById(id);
            entity.DeleteDate = DateTime.Now;
            entity.Status = Domain.Enum.Status.Deleted;
            return await _writeRepository.Delete(id);
        }

        public async Task<bool> Insert(T entity)
        {
            return await _writeRepository.Insert(entity);
        }



        public async Task<bool> Update(T entity)
        {
            return await _writeRepository.Update(entity);
        }
    }
}
