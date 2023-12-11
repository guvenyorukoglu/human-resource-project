using humanResourceProject.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.BaseServices
{
    public interface IBaseWriteService<T> where T : class, IBaseEntity, new()
    {
        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity</param>
        Task<bool> Insert(T entity);

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="entity"></param>
        Task<bool> Update(T entity);

        /// <summary>
        /// Makes soft deletion the entity by it's identifier. Sets it's status to inactive. Does not delete the entity from database.
        /// </summary>
        /// <param name="id">Identifier</param>
        Task<bool> Delete(Guid id);


    }
}

