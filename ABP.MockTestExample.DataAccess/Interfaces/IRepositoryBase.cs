// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepositoryBase.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IRepositoryBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MockTestExample.DataAccess.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// The RepositoryBase interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public interface IRepositoryBase<TEntity>
        where TEntity : class
    {
        IDbContext DbContext { get; set; }

        void Delete(object id);

        void Delete(TEntity entity);

        void Delete<TChild>(ICollection<TChild> entities)
            where TChild : class;

        IQueryable<TEntity> Fetch();

        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        ICollection<TEntity> GetAll();

        ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        Task<ICollection<TEntity>> GetAllAsync();

        Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        ICollection<TEntity> GetAllPaged<TType>(
            Expression<Func<TEntity, TType>> orderBy,
            int pageNumber,
            int pageSize,
            out int totalCount);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity GetById(object id);

        object Insert(TEntity entity);

        /// <summary>
        ///     Loads auxiliary data for the specified entity
        ///     from the repository.
        /// </summary>
        /// <param name="entity">
        ///     Entity to load auxiliary data into.
        /// </param>
        /// <returns>
        ///     Returns the loaded entity.
        /// </returns>
        TEntity LoadAuxData(TEntity entity);

        void Refresh();

        void SaveChanges();

        /// <summary>
        ///     Stores auxiliary data for the specified entity
        ///     into the repository.
        /// </summary>
        /// <param name="entity">
        ///     Entity to store auxiliary data into the
        ///     repository.
        /// </param>
        void StoreAuxData(TEntity entity);

        void Update(TEntity entity);

        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
    }
}