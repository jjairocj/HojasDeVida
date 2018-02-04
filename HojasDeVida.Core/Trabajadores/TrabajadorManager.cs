using System.Threading.Tasks;
using Abp.Domain.Services;

namespace HojasDeVida.Trabajadores
{
    public interface ITrabajadorManager : IDomainService
    {
        Task CrearAsync(Trabajador trabajador);
        Task ActualizarAsync(Trabajador trabajador);
        Task EliminarAsync(Trabajador trabajador);
    }

    public class TrabajadorManager : DomainService, ITrabajadorManager
    {
        private readonly ITrabajadorRepository _trabajadorRepository;

        public TrabajadorManager(ITrabajadorRepository trabajadorRepository)
        {
            _trabajadorRepository = trabajadorRepository;
        }

        public async Task CrearAsync(Trabajador trabajador)
        {
            await _trabajadorRepository.InsertAsync(trabajador);
            // Correo de bienvenida
        }

        public async Task ActualizarAsync(Trabajador trabajador)
        {
            await _trabajadorRepository.UpdateAsync(trabajador);
        }

        public async Task EliminarAsync(Trabajador trabajador)
        {
            await _trabajadorRepository.DeleteAsync(trabajador);
        }
    }
}
