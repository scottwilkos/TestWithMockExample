namespace MockTestExample.DataAccess
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using MockTestExample.DataAccess.Interfaces;
    using MockTestExample.DataAccess.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public bool LazyLoadingEnabled { get; set; }

        public bool ProxyCreationEnabled { get; set; }

        public void Commit()
        {
            this.SaveChanges();
        }

        public IDbSet<TEntity> GetDbSet<TEntity>()
            where TEntity : class
        {
            return this.Set<TEntity>();
        }
    }
}