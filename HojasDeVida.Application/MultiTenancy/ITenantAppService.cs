using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HojasDeVida.MultiTenancy.Dto;

namespace HojasDeVida.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
