using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace HojasDeVida.EntityFramework.Repositories
{
    public abstract class HojasDeVidaRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<HojasDeVidaDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected HojasDeVidaRepositoryBase(IDbContextProvider<HojasDeVidaDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class HojasDeVidaRepositoryBase<TEntity> : HojasDeVidaRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected HojasDeVidaRepositoryBase(IDbContextProvider<HojasDeVidaDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
