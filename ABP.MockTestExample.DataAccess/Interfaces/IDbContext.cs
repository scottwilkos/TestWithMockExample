namespace MockTestExample.DataAccess.Interfaces
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// The DbContext interface.
    /// </summary>
    public interface IDbContext : IDisposable
    {
        DbChangeTracker ChangeTracker { get; }

        DbContextConfiguration Configuration { get; }

        bool LazyLoadingEnabled { get; set; }

        bool ProxyCreationEnabled { get; set; }

        void Commit();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class;

        IDbSet<TEntity> GetDbSet<TEntity>()
            where TEntity : class;

        int SaveChanges();

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        DbSet Set(Type entityType);
    }
}
