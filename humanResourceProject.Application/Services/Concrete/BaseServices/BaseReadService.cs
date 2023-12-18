using humanResourceProject.Application.Services.Abstract.IBaseServices;
using humanResourceProject.Domain.Entities.Abstract;
using humanResourceProject.Domain.IRepository.BaseRepos;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Concrete.BaseServices
{
    public class BaseReadService<T> : IBaseReadService<T> where T : class, IBaseEntity, new()
    {
        private readonly IBaseReadRepository<T> _readRepository;
        public BaseReadService(IBaseReadRepository<T> readRepository)
        {
            _readRepository = readRepository;
        }


        public virtual Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return _readRepository.Any(expression);
        }

        public Task<List<T>> GetAll()
        {
            return _readRepository.GetAll();
        }

        public Task<T> GetById(Guid id)
        {
            return _readRepository.GetById(id);
        }

        public Task<T> GetDefault(Expression<Func<T, bool>> expression)
        {
            return _readRepository.GetDefault(expression);
        }

        public Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression)
        {
            return _readRepository.GetDefaults(expression);
        }

        public Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            return _readRepository.GetFilteredFirstOrDefault(select, where, orderBy, include);
        }

        public Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            return _readRepository.GetFilteredList(select, where, orderBy, include);
        }

        public Task<T> GetSingleDefault(Expression<Func<T, bool>> expression)
        {
            return _readRepository.GetSingleDefault(expression);
        }
    }
}
