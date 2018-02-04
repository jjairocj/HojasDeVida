using System;
using Abp.Domain.Repositories;

namespace HojasDeVida.Trabajadores
{
    public interface ITrabajadorRepository : IRepository<Trabajador, Guid>
    {
    }
}
