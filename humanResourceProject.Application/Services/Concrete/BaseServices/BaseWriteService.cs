using humanResourceProject.Application.Services.BaseServices;
using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.Entities.Concrete;
using humanResourceProject.Domain.IRepository;
using humanResourceProject.Infrastructure.Repositories.AppUserRepos;
using humanResourceProject.Infrastructure.Repositories.BaseRepos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task Delete(Guid id)
        {
            T entity = _readRepository.GetById(id);
            entity.DeleteDate = DateTime.Now;
            entity.Status = Domain.Enum.Status.Inactive;
            return _writeRepository.Delete(id);
        }

        public async Task Insert(T entity)
        {
           await _writeRepository.Insert(entity);
        }



        public async Task Update(T entity)
        {
            await _writeRepository.Update(entity);
        }
    }
}
