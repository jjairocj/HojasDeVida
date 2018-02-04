using System.Threading.Tasks;
using Abp.Application.Services;
using HojasDeVida.Sessions.Dto;

namespace HojasDeVida.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
