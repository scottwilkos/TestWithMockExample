// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the RepositoryBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MockTestExample.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using MockTestExample.DataAccess.Interfaces;

    /// <summary>
    ///     The repository base.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {
        private IDbSet<TEntity> _dbSet;

        public RepositoryBase(IDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public IDbContext DbContext { get; set; }

        public IDbSet<TEntity> DbSet
        {
            get
            {
                if (this._dbSet == null) this._dbSet = this.DbContext.GetDbSet<TEntity>();
                return this._dbSet;
            }
        }

        public virtual void Delete(object id)
        {
            this.DbSet.Remove(this.GetById(id));
        }

        public virtual void Delete(TEntity entity)
        {
            this.DbSet.Attach(entity);
            this.DbSet.Remove(entity);
        }

        public virtual void Delete<TChild>(ICollection<TChild> entities)
            where TChild : class
        {
            var e = entities.ToArray();
            for (int i = 0; i < e.Count(); i++) this.Delete(e[i]);
        }

        public virtual void Delete<TChild>(TChild entity)
            where TChild : class
        {
            this.DbContext.Set<TChild>().Remove(entity);
        }

        public virtual IQueryable<TEntity> Fetch()
        {
            return this.DbSet.AsQueryable();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.FirstOrDefault(predicate);
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return this.DbSet.ToList();
        }

        public virtual ICollection<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.Where(predicate).ToList();
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await this.DbSet.ToListAsync();
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.DbSet.Where(predicate).ToListAsync();
        }

        public virtual ICollection<TEntity> GetAllPaged<TType>(
            Expression<Func<TEntity, TType>> orderBy,
            int pageIndex,
            int pageSize,
            out int totalCount)
        {
            totalCount = this.DbSet.Count();

            return this.DbSet.OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize).ToList();
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.DbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual TEntity GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual object Insert(TEntity entity)
        {
            return this.DbSet.Add(entity);
        }

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
        /// <exception cref="NotImplementedException">
        ///     Always thrown in this base class because this
        ///     method must be implemented by derived classes.
        /// </exception>
        public virtual TEntity LoadAuxData(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Refresh()
        {
            /*foreach (var entity in DbContext.ChangeTracker.Entries<TEntity>())
            {
                entity.Reload();
            }*/
        }

        public virtual void SaveChanges()
        {
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException exEntityValidation)
            {
                string customMessage = string.Empty;
                foreach (var eve in exEntityValidation.EntityValidationErrors)
                {
                    customMessage = customMessage + string.Format(
                                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name,
                                        eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        customMessage = customMessage + string.Format(
                                            "- Property: \"{0}\", Error: \"{1}\"",
                                            ve.PropertyName,
                                            ve.ErrorMessage);
                }

                throw new DbEntityValidationException(
                    customMessage,
                    exEntityValidation.EntityValidationErrors,
                    exEntityValidation);
            }
            catch (DbUpdateException exDbUpdate)
            {
                if (exDbUpdate.InnerException != null && exDbUpdate.InnerException.InnerException != null
                    && exDbUpdate.InnerException.InnerException.Message.ToLower().Contains("duplicate"))
                    throw new DbUpdateException("Record already exists", exDbUpdate);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Stores auxiliary data for the specified entity
        ///     into the repository.
        /// </summary>
        /// <param name="entity">
        ///     Entity to store auxiliary data into the
        ///     repository.
        /// </param>
        /// <exception cref="NotImplementedException">
        ///     Always thrown in this base class because this
        ///     method must be implemented by derived classes.
        /// </exception>
        public virtual void StoreAuxData(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TEntity entity)
        {
            // if(DbContext.Entry(entity).State != EntityState.Detached)
            // DbContext.Entry(entity).State = EntityState.Detached;
            this.DbSet.Attach(entity);
            this.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            this.DbSet.Attach(entity);
            var item = this.DbContext.Entry(entity);

            properties.ToList().ForEach(property => { item.Property(property).IsModified = true; });
        }
    }
}