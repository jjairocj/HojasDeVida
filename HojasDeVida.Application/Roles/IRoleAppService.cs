using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HojasDeVida.Roles.Dto;

namespace HojasDeVida.Roles
{
    public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedResultRequestDto, CreateRoleDto, RoleDto>
    {
        Task<ListResultDto<PermissionDto>> GetAllPermissions();
    }
}
