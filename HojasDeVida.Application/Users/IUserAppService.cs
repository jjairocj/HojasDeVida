using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using HojasDeVida.Roles.Dto;
using HojasDeVida.Users.Dto;

namespace HojasDeVida.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedResultRequestDto, CreateUserDto, UpdateUserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();
    }
}