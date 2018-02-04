using System;
using Abp.EntityFramework;
using HojasDeVida.Trabajadores;

namespace HojasDeVida.EntityFramework.Repositories
{
    public class TrabajadorRepository : HojasDeVidaRepositoryBase<Trabajador, Guid>, ITrabajadorRepository
    {
        public TrabajadorRepository(IDbContextProvider<HojasDeVidaDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
