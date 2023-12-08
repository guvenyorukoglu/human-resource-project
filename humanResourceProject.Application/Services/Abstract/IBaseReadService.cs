using humanResourceProject.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace humanResourceProject.Application.Services.Abstract
{
    public interface IBaseReadService<T> where T : class, IBaseEntity, new()
    {
 

        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        /// <param name="expression">An expression to check for being empty.</param>
        /// <returns>true if the source sequence contains any elements; otherwise, false.</returns>
        Task<bool> Any(Expression<Func<T, bool>> expression);


        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>Returns all entities as a list.</returns>
        Task<List<T>> GetAll();

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <param name="expression">An expression to get the element.</param>
        /// <returns>Null if element is not found; otherwise, the first element in source.</returns>
        Task<T> GetDefault(Expression<Func<T, bool>> expression);

        Task<T> GetSingleDefault(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Returns the list of the elements of a sequence.
        /// </summary>
        /// <param name="expression">An expression to get the list of the elements.</param>
        /// <returns>Returns the list of the elements of a sequence.</returns>
        Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Gets the first element according to the parameters given.
        /// </summary>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns>Returns null if the element is not found; otherwise, the first element in source.</returns>
        Task<TResult> GetFilteredFirstOrDefault<TResult>(
            Expression<Func<T, TResult>> select, //select
            Expression<Func<T, bool>> where, //where
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, //orderBy
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);//Join  

        /// <summary>
        /// Gets all elements according to the parameters given.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns>Returns all elements according to the parameters given.</returns>
        Task<List<TResult>> GetFilteredList<TResult>(
            Expression<Func<T, TResult>> select, //select
            Expression<Func<T, bool>> where, //where
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, //orderBy
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);//Join
    }
}

