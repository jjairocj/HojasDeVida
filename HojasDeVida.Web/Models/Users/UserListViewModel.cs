using System.Collections.Generic;
using HojasDeVida.Roles.Dto;
using HojasDeVida.Users.Dto;

namespace HojasDeVida.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}